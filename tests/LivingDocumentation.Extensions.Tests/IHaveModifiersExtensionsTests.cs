namespace LivingDocumentation.Extensions.Tests
{
    [TestClass]
    public partial class IHaveModifiersExtensionsTests
    {
        [DataRow("IsInternal", Modifier.Public, false, DisplayName = "`IsInternal()` with a non-internal modifier should return `false`")]
        [DataRow("IsInternal", Modifier.Internal | Modifier.Static, true, DisplayName = "`IsInternal()` with an internal modifier should return `true`")]
        [DataRow("IsPublic", Modifier.Private, false, DisplayName = "`IsPublic()` with a non-public modifier should return `false`")]
        [DataRow("IsPublic", Modifier.Public | Modifier.Static, true, DisplayName = "`IsPublic()` with a public modifier should return `true`")]
        [DataRow("IsPrivate", Modifier.Public, false, DisplayName = "`IsPrivate()` with a non-private modifier should return `false`")]
        [DataRow("IsPrivate", Modifier.Private | Modifier.Static, true, DisplayName = "`IsPrivate()` with a private modifier should return `true`")]
        [DataRow("IsProtected", Modifier.Public, false, DisplayName = "`IsProtected()` with a non-protected modifier should return `false`")]
        [DataRow("IsProtected", Modifier.Protected | Modifier.Static, true, DisplayName = "`IsProtected()` with a protected modifier should return `true`")]
        [DataRow("IsStatic", Modifier.Public, false, DisplayName = "`IsStatic()` with a non-static modifier should return `false`")]
        [DataRow("IsStatic", Modifier.Public | Modifier.Static, true, DisplayName = "`IsStatic()` with a static modifier should return `true`")]
        [DataRow("IsAbstract", Modifier.Private, false, DisplayName = "`IsAbstract()` with a non-abstract modifier should return `false`")]
        [DataRow("IsAbstract", Modifier.Public | Modifier.Abstract, true, DisplayName = "`IsAbstract()` with a abstract modifier should return `true`")]
        [DataRow("IsOverride", Modifier.Public, false, DisplayName = "`IsOverride()` with a non-override modifier should return `false`")]
        [DataRow("IsOverride", Modifier.Public | Modifier.Override, true, DisplayName = "`IsOverride()` with a override modifier should return `true`")]
        [DataRow("IsReadonly", Modifier.Public, false, DisplayName = "`IsReadonly()` with a non-readonly modifier should return `false`")]
        [DataRow("IsReadonly", Modifier.Public | Modifier.Readonly, true, DisplayName = "`IsReadonly()` with a readonly modifier should return `true`")]
        [DataRow("IsAsync", Modifier.Public, false, DisplayName = "`IsAsync()` with a non-async modifier should return `false`")]
        [DataRow("IsAsync", Modifier.Public | Modifier.Async, true, DisplayName = "`IsAsync()` with a async modifier should return `true`")]
        [DataRow("IsConst", Modifier.Public, false, DisplayName = "`IsConst()` with a non-const modifier should return `false`")]
        [DataRow("IsConst", Modifier.Public | Modifier.Const, true, DisplayName = "`IsConst()` with a const modifier should return `true`")]
        [DataRow("IsSealed", Modifier.Public, false, DisplayName = "`IsSealed()` with a non-sealed modifier should return `false`")]
        [DataRow("IsSealed", Modifier.Public | Modifier.Sealed, true, DisplayName = "`IsSealed()` with a sealed modifier should return `true`")]
        [DataRow("IsVirtual", Modifier.Public, false, DisplayName = "`IsVirtual()` with a non-virtual modifier should return `false`")]
        [DataRow("IsVirtual", Modifier.Public | Modifier.Virtual, true, DisplayName = "`IsVirtual()` with a virtual modifier should return `true`")]
        [DataRow("IsExtern", Modifier.Public, false, DisplayName = "`IsExtern()` with a non-extern modifier should return `false`")]
        [DataRow("IsExtern", Modifier.Public | Modifier.Extern, true, DisplayName = "`IsExtern()` with a extern modifier should return `true`")]
        [DataRow("IsNew", Modifier.Public, false, DisplayName = "`IsNew()` with a non-new modifier should return `false`")]
        [DataRow("IsNew", Modifier.Public | Modifier.New, true, DisplayName = "`IsNew()` with a new modifier should return `true`")]
        [DataRow("IsUnsafe", Modifier.Public, false, DisplayName = "`IsUnsafe()` with a non-unsafe modifier should return `false`")]
        [DataRow("IsUnsafe", Modifier.Public | Modifier.Unsafe, true, DisplayName = "`IsUnsafe()` with a unsafe modifier should return `true`")]
        [DataRow("IsPartial", Modifier.Public, false, DisplayName = "`IsPartial()` with a non-partial modifier should return `false`")]
        [DataRow("IsPartial", Modifier.Public | Modifier.Partial, true, DisplayName = "`IsPartial()` with a partial modifier should return `true`")]
        [TestMethod]
        public void ModifierMethodsShouldReturnCorrectValues(string methodName, Modifier modifiers, bool expectation)
        {
            var method = typeof(IHaveModifiersExtensions).GetMethods().Single(m => m.Name == methodName);
            var parameters = new[] { new Mod { Modifiers = modifiers } };

            // Act
            var result = (bool)method.Invoke(null, parameters);

            // Assert
            result.Should().Be(expectation);
        }

        private class Mod : IHaveModifiers
        {
            public Modifier Modifiers { get; set; }
        }
    }
}
