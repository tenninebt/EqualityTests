using System;
using System.Globalization;
using System.Reflection;
using EqualityTests.Extensions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;

namespace EqualityTests.Assertions
{
    internal class EqualsTypeAssertion<T> : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder _builder;
        
        public EqualsTypeAssertion(ISpecimenBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Verifies that calling `x.Equals(AnotherType obj)` on the method returns false, if the supplied
        /// method is an override of the <see cref="M:System.Object.Equals(System.Object)" />.
        /// </summary>
        /// <param name="methodInfo">The Equals method to verify</param>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!(methodInfo.ReflectedType == null) && methodInfo.IsObjectEqualsOverrideMethod() && _builder.Create<T>().Equals(new object()))
            {
                throw new EqualsOverrideException(string.Format(CultureInfo.CurrentCulture,
                    "The type '{0}' overrides the object.Equals(object) method incorrectly, calling x.Equals(AnotherType obj) should return false.", methodInfo.DeclaringType?.FullName));
            }
        }
    }
}