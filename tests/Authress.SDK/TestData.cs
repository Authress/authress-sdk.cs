using Xunit;

namespace Authress.SDK.UnitTests
{
    /// <summary>
    /// Represents a set of data for a theory. Data can be added to the data set using the collection initializer syntax.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TestData<T> : TheoryData<string, T>
    {
        // /// <summary>
        // /// Adds a theory to the Test
        // /// </summary>
        // /// <param name="testName">Name of the Test</param>
        // /// <param name="testObject">Data used in the Test</param>
        // public void Add(string testName, T testObject)
        // {
        //     AddRow(testName, testObject);
        // }
    }
}
