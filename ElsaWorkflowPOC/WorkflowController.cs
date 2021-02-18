using Elsa.Persistence;
using Elsa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaWorkflowPOC
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowInvoker _workflowInvoker;
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        public WorkflowController(IWorkflowInvoker workflowInvoker, IWorkflowDefinitionStore workflowDefinitionStore)
        {
            this._workflowInvoker = workflowInvoker;
            this._workflowDefinitionStore = workflowDefinitionStore;
        }

        [Route("Start")]
        [HttpGet]
        public async Task Start()
        {
            var workflowInstance = await this._workflowInvoker.StartAsync<NumberGenerationWorkflow>();
            var definition = workflowInstance.Workflow.Definition;
            definition.Name = "Test Workflow";
            definition.Description = "Check if we can track changes of programmatically created workflows";
            await this._workflowDefinitionStore.AddAsync(definition);
        }
    }
}
