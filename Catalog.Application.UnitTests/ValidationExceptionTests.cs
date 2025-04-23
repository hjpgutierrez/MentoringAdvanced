using Catalog.Application.Common.Exceptions;
using FluentValidation.Results;

namespace Catalog.Application.UnitTests
{
    public class ValidationExceptionTests
    {
        [Fact]
        public void DefaultConstructor_InitializesEmptyErrors()
        {
            // Act
            var exception = new ValidationException();

            // Assert
            Assert.Equal("One or more validation failures have occurred.", exception.Message);
            Assert.NotNull(exception.Errors);
            Assert.Empty(exception.Errors);
        }

        [Fact]
        public void Constructor_WithFailures_GroupsFailuresByPropertyName()
        {
            // Arrange
            var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", "Error1"),
            new ValidationFailure("Property1", "Error2"),
            new ValidationFailure("Property2", "Error3")
        };

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.NotNull(exception.Errors);
            Assert.Equal(2, exception.Errors.Count); // Two properties
            Assert.True(exception.Errors.ContainsKey("Property1"));
            Assert.True(exception.Errors.ContainsKey("Property2"));
            Assert.Equal(new[] { "Error1", "Error2" }, exception.Errors["Property1"]);
            Assert.Equal(new[] { "Error3" }, exception.Errors["Property2"]);
        }

        [Fact]
        public void Constructor_WithEmptyFailures_InitializesEmptyErrors()
        {
            // Arrange
            var failures = Enumerable.Empty<ValidationFailure>();

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.NotNull(exception.Errors);
            Assert.Empty(exception.Errors);
        }

        [Fact]
        public void Constructor_WithMultipleFailuresForSameProperty_GroupsErrorsCorrectly()
        {
            // Arrange
            var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", "Error1"),
            new ValidationFailure("Property1", "Error2"),
            new ValidationFailure("Property1", "Error3")
        };

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.NotNull(exception.Errors);
            Assert.Single(exception.Errors); // Only one property
            Assert.True(exception.Errors.ContainsKey("Property1"));
            Assert.Equal(new[] { "Error1", "Error2", "Error3" }, exception.Errors["Property1"]);
        }
    }
}
