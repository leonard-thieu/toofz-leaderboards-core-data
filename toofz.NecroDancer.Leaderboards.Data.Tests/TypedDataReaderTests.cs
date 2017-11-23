using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class TypedDataReaderTests
    {
        public TypedDataReaderTests()
        {
            using (var db = new TestContext())
            {
                entityType = db.Model.FindEntityType(typeof(TestDto));
            }
        }

        private readonly IEntityType entityType;

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                using (var db = new TestContext())
                {
                    var entityType = db.Model.FindEntityType(typeof(TestDto));
                    var items = new List<TestDto>();

                    // Act
                    var reader = new TypedDataReader<TestDto>(entityType, items);

                    // Assert
                    Assert.IsAssignableFrom<TypedDataReader<TestDto>>(reader);
                }
            }
        }

        public class GetValueMethod : TypedDataReaderTests
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
                var reader = new TypedDataReader<TestDto>(entityType, items);
                reader.Read();

                // Act
                var value = reader.GetValue(0);

                // Assert
                Assert.Equal("TEST", value);
            }
        }

        public class GetOrdinalMethod : TypedDataReaderTests
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
                var reader = new TypedDataReader<TestDto>(entityType, items);

                // Act
                var ordinal = reader.GetOrdinal("nullable_text");

                // Assert
                Assert.Equal(2, ordinal);
            }
        }

        public class FieldCountProperty : TypedDataReaderTests
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
                var reader = new TypedDataReader<TestDto>(entityType, items);

                // Act
                var fieldCount = reader.FieldCount;

                // Assert
                Assert.Equal(5, fieldCount);
            }
        }

        public class ReadMethod : TypedDataReaderTests
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
                var reader = new TypedDataReader<TestDto>(entityType, items);

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
                var reader = new TypedDataReader<TestDto>(entityType, items);

                // Act
                reader.Read();
                var isSuccessful = reader.Read();

                // Assert
                Assert.False(isSuccessful);
            }
        }

        private class TestContext : DbContext
        {
            public DbSet<TestDto> TestDtos => Set<TestDto>();
        }

        private class TestDto
        {
            public string KeyPart1 { get; set; }
            public short KeyPart2 { get; set; }
            public string Text { get; set; }
            public int Number { get; set; }
            public DateTimeOffset Date { get; set; }
        }
    }
}
