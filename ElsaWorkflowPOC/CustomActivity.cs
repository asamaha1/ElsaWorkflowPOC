using Elsa;
using Elsa.Attributes;
using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaWorkflowPOC
{
    [ActivityDefinition(Outcomes = new[] { "Even", "Odd", OutcomeNames.False })]
    public class GenerateARandomNumberActivity : Activity
    {
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            try
            {
                var random = new Random();
                var number = random.Next();


                context.SetVariable("generatedNumber", number);

                return Outcome(number % 2 == 0 ? "Even" : "Odd");
            }
            catch (Exception e)
            {
                return Outcome(OutcomeNames.False);
            }
        }
    }
    public class ToExecuteInCaseOfFailureActivity : Activity
    {
        public ToExecuteInCaseOfFailureActivity()
        {
            this.Name = "FailureActivity";
        }
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            return base.OnExecute(context);
        }
    }

    public class ToExecuteInCaseNumberIsEvenActivity : Activity
    {
        public ToExecuteInCaseNumberIsEvenActivity()
        {
            this.Name = "EvenActivity";
        }
        protected override bool OnCanExecute(WorkflowExecutionContext context)
        {
            // Permission or validation of some kind
            return base.OnCanExecute(context);
        }
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            var receivedInput = context.Workflow.Scope.Variables["generatedNumber"].Value;
            Console.Write(receivedInput);
            return Finish();
        }
    }

    public class ToExecuteInCaseNumberIsOddActivity : Activity
    {
        public ToExecuteInCaseNumberIsOddActivity()
        {
            this.Name = "OddActivity";
        }
        protected override bool OnCanExecute(WorkflowExecutionContext context)
        {
            // Permission or validation of some kind
            return base.OnCanExecute(context);
        }
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            var receivedInput = context.Workflow.Scope.Variables["generatedNumber"].Value;
            Console.Write(receivedInput);
            return Finish();
        }
    }
}
