using Xunit;

namespace PaymentGateway.TestKernel.Helpers
{
    public static class ReflectionHelper
    {
        public static void UpdatePrivateProperty(object input, string propertyName, object value)
        {
            var property = input.GetType()
                                .GetProperty(propertyName);

            Assert.True(property != null);

            property.SetValue(input, value, null);
        }
    }
}
