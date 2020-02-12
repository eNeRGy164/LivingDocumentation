using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void Color_NullConstructor_ToString_Should_ReturnEmptyString()
        {
            // Assign
            var color = new Color(null);

            // Assert
            color.ToString().Should().Be("");
        }

        [TestMethod]
        public void Color_StringConstructorWithHashTag_ToString_Should_ReturnValueWithHashTag()
        {
            // Assign
            var color = new Color("#AliceBlue");

            // Assert
            color.ToString().Should().Be("#AliceBlue");
        }

        [TestMethod]
        public void Color_StringConstructorWithoutHashTag_ToString_Should_ReturnValueWithHashTag()
        {
            // Assign
            var color = new Color("AliceBlue");

            // Assert
            color.ToString().Should().Be("#AliceBlue");
        }

        [TestMethod]
        public void Color_NamedColorEnumConstructor_ToString_Should_ReturnValueWithHashTag()
        {
            // Assign
            var color = new Color(NamedColor.AliceBlue);

            // Assert
            color.ToString().Should().Be("#AliceBlue");
        }

        [TestMethod]
        public void Color_StringCast_ToString_Should_ReturnValueWithHashTag()
        {
            // Assign
            var value = "AliceBlue";

            // Act
            var color = (Color)value;

            // Assert
            color.ToString().Should().Be("#AliceBlue");
        }

        [TestMethod]
        public void Color_NamedColorEnumCast_ToString_Should_ReturnValueWithHashTag()
        {
            // Assign
            var value = NamedColor.AliceBlue;

            // Act
            var color = (Color)value;

            // Assert
            color.ToString().Should().Be("#AliceBlue");
        }
    }
}
