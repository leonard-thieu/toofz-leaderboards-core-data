using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class TypedDataReaderTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var items = new List<TestDto>();

                // Act
                var reader = new TypedDataReader<TestDto>(TestDto.PropertyMappings, items);

                // Assert
                Assert.IsAssignableFrom<TypedDataReader<TestDto>>(reader);
            }
        }

        public class GetValueMethod
        {
            [Fact]
            public void ReturnsValue()
            {
                // Arrange
                var items = new List<TestDto>
                {
                    new TestDto
                    {
                        KeyPart1 = "TEST",
                        KeyPart2 = 1,
                        Text = "some text here 1",
                        Number = 1,
                        Date = new DateTimeOffset(new DateTime(2010, 11, 14, 12, 0, 0), TimeSpan.FromHours(1)),
                    },
                };
                var reader = new TypedDataReader<TestDto>(TestDto.PropertyMappings, items);
                reader.Read();

                // Act
                var value = reader.GetValue(0);

                // Assert
                Assert.Equal("TEST", value);
            }
        }

        public class GetOrdinalMethod
        {
            [Fact]
            public void ReturnsOrdinal()
            {
                // Arrange
                var items = new List<TestDto>
                {
                    new TestDto
                    {
                        KeyPart1 = "TEST",
                        KeyPart2 = 1,
                        Text = "some text here 1",
                        Number = 1,
                        Date = new DateTimeOffset(new DateTime(2010, 11, 14, 12, 0, 0), TimeSpan.FromHours(1)),
                    },
                };
                var reader = new TypedDataReader<TestDto>(TestDto.PropertyMappings, items);

                // Act
                var ordinal = reader.GetOrdinal("nullable_text");

                // Assert
                Assert.Equal(2, ordinal);
            }
        }

        public class FieldCountProperty
        {
            [Fact]
            public void ReturnsFieldCount()
            {
                // Arrange
                var items = new List<TestDto>
                {
                    new TestDto
                    {
                        KeyPart1 = "TEST",
                        KeyPart2 = 1,
                        Text = "some text here 1",
                        Number = 1,
                        Date = new DateTimeOffset(new DateTime(2010, 11, 14, 12, 0, 0), TimeSpan.FromHours(1)),
                    },
                };
                var reader = new TypedDataReader<TestDto>(TestDto.PropertyMappings, items);

                // Act
                var fieldCount = reader.FieldCount;

                // Assert
                Assert.Equal(5, fieldCount);
            }
        }

        public class ReadMethod
        {
            [Fact]
            public void ReadIsSuccessful_ReturnsTrue()
            {
                // Arrange
                var items = new List<TestDto>
                {
                    new TestDto
                    {
                        KeyPart1 = "TEST",
                        KeyPart2 = 1,
                        Text = "some text here 1",
                        Number = 1,
                        Date = new DateTimeOffset(new DateTime(2010, 11, 14, 12, 0, 0), TimeSpan.FromHours(1)),
                    },
                };
                var reader = new TypedDataReader<TestDto>(TestDto.PropertyMappings, items);

                // Act
                var isSuccessful = reader.Read();

                // Assert
                Assert.True(isSuccessful);
            }

            [Fact]
            public void ReadIsNotSuccessful_ReturnsFalse()
            {
                // Arrange
                var items = new List<TestDto>
                {
                    new TestDto
                    {
                        KeyPart1 = "TEST",
                        KeyPart2 = 1,
                        Text = "some text here 1",
                        Number = 1,
                        Date = new DateTimeOffset(new DateTime(2010, 11, 14, 12, 0, 0), TimeSpan.FromHours(1)),
                    },
                };
                var reader = new TypedDataReader<TestDto>(TestDto.PropertyMappings, items);

                // Act
                reader.Read();
                var isSuccessful = reader.Read();

                // Assert
                Assert.False(isSuccessful);
            }
        }

        private class TestDto
        {
            public static IEnumerable<ScalarPropertyMapping> PropertyMappings = new[]
            {
                new ScalarPropertyMapping(
                    EdmProperty.CreatePrimitive("KeyPart1", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String)),
                    EdmProperty.CreatePrimitive("key_part_1", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String))),
                new ScalarPropertyMapping(
                    EdmProperty.CreatePrimitive("KeyPart2", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32)),
                    EdmProperty.CreatePrimitive("key_part_2", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32))),
                new ScalarPropertyMapping(
                    EdmProperty.CreatePrimitive("Text", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String)),
                    EdmProperty.CreatePrimitive("nullable_text", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String))),
                new ScalarPropertyMapping(
                    EdmProperty.CreatePrimitive("Number", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32)),
                    EdmProperty.CreatePrimitive("nullable_number", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32))),
                new ScalarPropertyMapping(
                    EdmProperty.CreatePrimitive("Date", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.DateTimeOffset)),
                    EdmProperty.CreatePrimitive("nullable_datetimeoffset", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.DateTimeOffset))),
            };

            public string KeyPart1 { get; set; }
            public short KeyPart2 { get; set; }
            public string Text { get; set; }
            public int Number { get; set; }
            public DateTimeOffset Date { get; set; }
        }
    }
}
