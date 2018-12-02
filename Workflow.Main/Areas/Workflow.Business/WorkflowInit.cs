﻿using System;
using System.Configuration;
using System.Reflection;
using System.Xml.Linq;
using OptimaJet.Workflow.Core.Builder;
using OptimaJet.Workflow.Core.Bus;
using OptimaJet.Workflow.Core.Parser;
using OptimaJet.Workflow.Core.Runtime;
using OptimaJet.Workflow.Core.Persistence;
using WF.Sample.Business.Helpers;

namespace WF.Sample.Business.Workflow
{
    public static class WorkflowInit
    {
        private static volatile WorkflowRuntime _runtime;

        private static readonly object _sync = new object();

        public static WorkflowRuntime Runtime
        {
            get
            {
                if (_runtime == null)
                {
                    lock (_sync)
                    {
                        if (_runtime == null)
                        {

                            var connectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                           
                            var provider = new OptimaJet.Workflow.DbPersistence.MSSQLProvider(connectionString);

                            var builder = new WorkflowBuilder<XElement>(provider,
                                new XmlWorkflowParser(),
                                provider
                                ).WithDefaultCache();

                            _runtime = new WorkflowRuntime()
                                .WithBuilder(builder)
                                .WithActionProvider(new WorkflowActions())
                                .WithRuleProvider(new WorkflowRule())
                                .WithPersistenceProvider(provider)
                                .WithTimerManager(new TimerManager())
                                .WithBus(new NullBus())
                                .SwitchAutoUpdateSchemeBeforeGetAvailableCommandsOn()
                                .RegisterAssemblyForCodeActions(Assembly.GetExecutingAssembly())
                                .RegisterAssemblyForCodeActions(Assembly.GetAssembly(typeof(System.Data.Linq.DataContext)))
                                .Start();

                            _runtime.ProcessStatusChanged += _runtime_ProcessStatusChanged;
                        }
                    }
                }

                return _runtime;
            }
        }

        static void _runtime_ProcessStatusChanged(object sender, ProcessStatusChangedEventArgs e)
        {
            if (e.NewStatus != ProcessStatus.Idled && e.NewStatus != ProcessStatus.Finalized)
                return;

            if (string.IsNullOrEmpty(e.SchemeCode))
                return;

            WorkflowActions.DeleteEmptyPreHistory(e.ProcessId);
            _runtime.PreExecuteFromCurrentActivity(e.ProcessId);

            //Inbox
            using (var context = new DataModelDataContext())
            {
                context.DropWorkflowInbox(e.ProcessId);
                context.SubmitChanges();
            }

            if (e.NewStatus != ProcessStatus.Finalized)
            {
                var d = new Action<ProcessStatusChangedEventArgs>(PreExecuteAndFillInbox);
                d.BeginInvoke(e, FillInboxCallback, null);
            }

            //Change state name
            var nextState = WorkflowInit.Runtime.GetLocalizedStateName(e.ProcessId, e.ProcessInstance.CurrentState);
            using (var context = new DataModelDataContext())
            {
                var document = DocumentHelper.Get(e.ProcessId, context);
                if (document == null)
                    return;

                document.StateName = nextState;
                context.SubmitChanges();
            }
        }

        #region Inbox
        private static void FillInboxCallback(IAsyncResult ar)
        {
        }

        private static void PreExecuteAndFillInbox(ProcessStatusChangedEventArgs e)
        {
            var processId = e.ProcessId;

            using (var context = new DataModelDataContext())
            {
                FillInbox(processId, context);

                context.SubmitChanges();
            }
        }

        private static void FillInbox(Guid processId, DataModelDataContext context)
        {
            var newActors = Runtime.GetAllActorsForDirectCommandTransitions(processId);
            foreach (var newActor in newActors)
            {
                var newInboxItem = new WorkflowInbox() { Id = Guid.NewGuid(), IdentityId = new Guid(newActor), ProcessId = processId };
                context.WorkflowInboxes.InsertOnSubmit(newInboxItem);
            }
        }

        public static void RecalcInbox()
        {
            using (var context = new DataModelDataContext())
            {
                foreach (var d in WF.Sample.Business.Helpers.DocumentHelper.GetAll())
                {
                    Guid id = d.Id;
                    try
                    {
                        if (_runtime.IsProcessExists(id))
                        {
                            _runtime.UpdateSchemeIfObsolete(id);
                            context.DropWorkflowInbox(id);
                            FillInbox(id, context);

                            context.SubmitChanges();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Unable to calculate the inbox for process Id = {0}", id), ex);
                    }

                }
            }
        }
        #endregion
    }
}
