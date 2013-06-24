using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SimpleConfig.BindingStrategies;

namespace SimpleConfig.Tests.BindingStrategies
{
    [TestFixture]
    public class EnumerableBindingStrategyTests
    {
        [TestCase(typeof(IEnumerable), typeof(List<object>))]
        [TestCase(typeof(ICollection), typeof(List<object>))]
        [TestCase(typeof(IList), typeof(List<object>))]
        public void CollectionFor_NonGenericInterfaceTypesShouldNotBeSupported(Type requested, Type expectedInstanceType)
        {
            Action action = ()=>new EnumerableBindingStrategy().CollectionFor(requested);
            action.ShouldThrow<InvalidOperationException>();
        }

        [TestCase(typeof(IEnumerable<string>), typeof(List<string>))]
        [TestCase(typeof(ICollection<string>), typeof(List<string>))]
        [TestCase(typeof(IList<string>), typeof(List<string>))]
        public void CollectionFor_GenericInterfaceTypesShouldBeSupported(Type requested, Type expectedInstanceType)
        {
            var createdInstance = new EnumerableBindingStrategy().CollectionFor(requested);
            createdInstance.GetType().Should().Be(expectedInstanceType);
        }

        [TestCase(typeof(List<string>), typeof(List<string>))]
        [TestCase(typeof(SuperList<string>), typeof(SuperList<string>))]
        public void CollectionFor_GenericConcreeteTypesShouldBeSupported_EvenIfTheyAreSubclassesOfList(Type requested, Type expectedInstanceType)
        {
            var createdInstance = new EnumerableBindingStrategy().CollectionFor(requested);
            createdInstance.GetType().Should().Be(expectedInstanceType);
        }

        [TestCase(typeof(SuperEnumerable<string>), typeof(SuperEnumerable<string>))]
        [TestCase(typeof(SortedSet<string>), typeof(SortedSet<string>))]
        public void CollectionFor_GenericConcreeteTypesShouldBeSupported_EvenIfTheyAreNotSubclassesOfList(Type requested, Type expectedInstanceType)
        {
            var createdInstance = new EnumerableBindingStrategy().CollectionFor(requested);
            createdInstance.GetType().Should().Be(expectedInstanceType);
        }

        [Test]
        public void CollectionFor_ShouldThrowAUsefulExceptionForTypesThatAreNotGenericIEnumerable()
        {
            var strategy = new EnumerableBindingStrategy();
            Action act = () => strategy.CollectionFor(GetType());
            act.ShouldThrow<InvalidOperationException>().WithMessage("Type does not implement IEnumerable<>: SimpleConfig.Tests.BindingStrategies.EnumerableBindingStrategyTests, SimpleConfig.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
        }

        public class SuperList<T> : List<T>{}
        public class SuperEnumerable<T> : IEnumerable<T>
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<T> GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
