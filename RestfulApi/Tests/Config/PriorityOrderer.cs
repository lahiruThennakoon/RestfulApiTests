using Xunit.Abstractions;
using Xunit.Sdk;

namespace RestfulApi.Tests.Config
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var sorted = testCases.OrderBy(testCase =>
            {
                var attr = testCase.TestMethod.Method
                    .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName)
                    .FirstOrDefault();
                return attr == null ? 0 : attr.GetNamedArgument<int>("Priority");
            });
            return sorted;
        }
    }
}