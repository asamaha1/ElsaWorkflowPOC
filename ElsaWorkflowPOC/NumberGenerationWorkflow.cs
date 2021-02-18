using Elsa;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.ControlFlow.Activities;
using Elsa.Expressions;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaWorkflowPOC
{
    public class NumberGenerationWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            var workflowDefinition = builder
                .StartWith<WriteLine>(x => x.TextExpression = new LiteralExpression("Starting random number Workflow"))
                .Then<GenerateARandomNumberActivity>(
                    setup: null,
                    activity =>
                    {
                        activity.When("Even").Then<ToExecuteInCaseNumberIsEvenActivity>();
                        activity.When("Odd").Then<ToExecuteInCaseNumberIsOddActivity>();
                        activity.When(OutcomeNames.False).Then<ToExecuteInCaseOfFailureActivity>();
                    }
                );
        }
    }
}
