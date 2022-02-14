namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class DeserializationTests
{
    [TestMethod]
    public void NoTypes_Should_GiveEmptyArray()
    {
        // Assign
        var json = @"[]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types.Should().BeEmpty();
    }

    [TestMethod]
    public void AClassWithoutAModifierShouldBeInternalByDefault()
    {
        // Assign
        var json = @"[{""FullName"":""Test""}]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types.Should().HaveCount(1);
        types[0].Should().NotBeNull();
        types[0].Type.Should().Be(TypeType.Class);
        types[0].FullName.Should().Be("Test");
        types[0].Modifiers.Should().Be(Modifier.Internal);
    }

    [TestMethod]
    public void Collections_Should_NotBeNull()
    {
        // Assign
        var json = @"[{""FullName"":""Test""}]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Fields.Should().BeEmpty();
        types[0].Constructors.Should().BeEmpty();
        types[0].Properties.Should().BeEmpty();
        types[0].Methods.Should().BeEmpty();
        types[0].EnumMembers.Should().BeEmpty();
        types[0].Events.Should().BeEmpty();
    }

    [DataRow(00_001, Modifier.Internal, DisplayName = "A serialized value of `1` should be parsed as an `internal` modifier")]
    [DataRow(00_002, Modifier.Public, DisplayName = "A serialized value of `2` should be parsed as a `public` modifier")]
    [DataRow(00_004, Modifier.Private, DisplayName = "A serialized value of `4` should be parsed as a `private` modifier")]
    [DataRow(00_008, Modifier.Protected, DisplayName = "A serialized value of `8` should be parsed as a `protected` modifier")]
    [DataRow(00_012, Modifier.Private | Modifier.Protected, DisplayName = "A serialized value of `12` should be parsed as a `private` and `protected` modifier")]
    [DataRow(00_016, Modifier.Static, DisplayName = "A serialized value of `16` should be parsed as a `static` modifier")]
    [DataRow(00_032, Modifier.Abstract, DisplayName = "A serialized value of `32` should be parsed as an `abstract` modifier")]
    [DataRow(00_064, Modifier.Override, DisplayName = "A serialized value of `64` should be parsed as an `override` modifier")]
    [DataRow(00_128, Modifier.Readonly, DisplayName = "A serialized value of `128` should be parsed as a `readonly` modifier")]
    [DataRow(00_256, Modifier.Async, DisplayName = "A serialized value of `256` should be parsed as an `async` modifier")]
    [DataRow(00_512, Modifier.Const, DisplayName = "A serialized value of `512` should be parsed as a `const` modifier")]
    [DataRow(01_024, Modifier.Sealed, DisplayName = "A serialized value of `1024` should be parsed as a `sealed` modifier")]
    [DataRow(02_048, Modifier.Virtual, DisplayName = "A serialized value of `2048` should be parsed as a `virtual` modifier")]
    [DataRow(04_096, Modifier.Extern, DisplayName = "A serialized value of `4096` should be parsed as an `extern` modifier")]
    [DataRow(08_192, Modifier.New, DisplayName = "A serialized value of `8192` should be parsed as a `new` modifier")]
    [DataRow(16_384, Modifier.Unsafe, DisplayName = "A serialized value of `16384` should be parsed as an `unsafe` modifier")]
    [DataRow(32_768, Modifier.Partial, DisplayName = "A serialized value of `32768` should be parsed as a `partial` modifier")]
    [TestMethod]
    public void ModifiersShouldBeDeserializedCorrectly(int value, Modifier modifier)
    {
        // Assign
        var json = @$"[{{""Modifiers"":{value},""FullName"":""Test""}}]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    public void MembersOfAClassWithoutAModifierShouldBePrivateByDefault()
    {
        // Assign
        var json = @"[{""FullName"":""Test"",""Methods"":[{""Name"":""Method""}]}]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods.Should().HaveCount(1);
        types[0].Methods[0].Should().NotBeNull();
        types[0].Methods[0].Name.Should().Be("Method");
        types[0].Methods[0].Modifiers.Should().Be(Modifier.Private);
    }

    [TestMethod]
    public void AttributeCollection_Should_GiveAttributeWithNameAndType()
    {
        // Assign
        var json = @"[{""FullName"":""Test"",""Attributes"":[{""Type"":""System.ObsoleteAttribute"",""Name"":""System.Obsolete""}]}]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Attributes.Should().HaveCount(1);
        types[0].Attributes[0].Should().NotBeNull();
        types[0].Attributes[0].Type.Should().Be("System.ObsoleteAttribute");
        types[0].Attributes[0].Name.Should().Be("System.Obsolete");
    }

    [TestMethod]
    public void AttributeArgumentCollection_Should_GiveAttributeArgumentWithName_TypeAndValue()
    {
        // Assign
        var json = @"[{""FullName"":""Test"",""Attributes"":[{""Type"":""System.ObsoleteAttribute"",""Name"":""System.Obsolete"",""Arguments"":[{""Name"":""\""Reason\"""",""Type"":""string"",""Value"":""Reason""}]}]}]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Attributes[0].Arguments.Should().HaveCount(1);
        types[0].Attributes[0].Arguments[0].Should().NotBeNull();
        types[0].Attributes[0].Arguments[0].Type.Should().Be("string");
        types[0].Attributes[0].Arguments[0].Name.Should().Be(@"""Reason""");
        types[0].Attributes[0].Arguments[0].Value.Should().Be(@"Reason");
    }

    [TestMethod]
    public void AStatementInAMethodBodyShouldHaveTheMethodAsParent()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Methods"":[{
                ""Name"":""Method"",
                ""Statements"":[{
                    ""$type"":""LivingDocumentation.InvocationDescription, LivingDocumentation.Descriptions"",
                    ""ContainingType"": ""Test"",
                    ""Name"": ""TestMethod""
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods[0].Statements[0].Parent.Should().Be(types[0].Methods[0]);
    }

    [TestMethod]
    public void AStatementInAConstructorBodyShouldHaveTheConstructorAsParent()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Constructors"":[{
                ""Name"":""Constructor"",
                ""Statements"":[{
                    ""$type"":""LivingDocumentation.InvocationDescription, LivingDocumentation.Descriptions"",
                    ""ContainingType"": ""Test"",
                    ""Name"": ""TestMethod""
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Constructors[0].Statements[0].Parent.Should().Be(types[0].Constructors[0]);
    }

    [TestMethod]
    public void AnIfElseSectionShouldHaveTheIfAsParent()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Methods"":[{
                ""Name"":""Method"",
                ""Statements"":[{
                    ""$type"": ""LivingDocumentation.If, LivingDocumentation.Statements"",
                    ""Sections"":[{}]
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods[0].Statements[0].Should().BeOfType<If>();

        var @if = (If)types[0].Methods[0].Statements[0];
        @if.Sections[0].Parent.Should().Be(@if);
    }

    [TestMethod]
    public void AnIfElseConditionShouldBeParsedCorrectly()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Methods"":[{
                ""Name"":""Method"",
                ""Statements"":[{
                    ""$type"": ""LivingDocumentation.If, LivingDocumentation.Statements"",
                    ""Sections"":[{""Condition"": ""true""}]
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods[0].Statements[0].Should().BeOfType<If>();

        var @if = (If)types[0].Methods[0].Statements[0];
        @if.Sections[0].Condition.Should().Be("true");
    }[TestMethod]
    public void AStatementInAnIfElseSectionShouldHaveTheIfElseSectionAsParent()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Methods"":[{
                ""Name"":""Method"",
                ""Statements"":[{
                    ""$type"": ""LivingDocumentation.If, LivingDocumentation.Statements"",
                    ""Sections"":[{
                        ""Statements"":[{
                            ""$type"":""LivingDocumentation.InvocationDescription, LivingDocumentation.Descriptions"",
                            ""ContainingType"": ""Test"",
                            ""Name"": ""TestMethod""
                        }]
                    }]
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods[0].Statements[0].Should().BeOfType<If>();

        var @if = (If)types[0].Methods[0].Statements[0];
        @if.Sections[0].Statements[0].Parent.Should().Be(@if.Sections[0]);
    }

    [TestMethod]
    public void ASwitchSectionShouldHaveTheSwitchAsParent()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Methods"":[{
                ""Name"":""Method"",
                ""Statements"":[{
                    ""$type"": ""LivingDocumentation.Switch, LivingDocumentation.Statements"",
                    ""Sections"":[{}]
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods[0].Statements[0].Should().BeOfType<Switch>();

        var @switch = (Switch)types[0].Methods[0].Statements[0];
        @switch.Sections[0].Parent.Should().Be(@switch);
    }

    [TestMethod]
    public void ASwitchExpressionShouldBeParsedCorrectly()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Methods"":[{
                ""Name"":""Method"",
                ""Statements"":[{
                    ""$type"": ""LivingDocumentation.Switch, LivingDocumentation.Statements"",
                    ""Expression"":""type""
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods[0].Statements[0].Should().BeOfType<Switch>();

        var @switch = (Switch)types[0].Methods[0].Statements[0];
        @switch.Expression.Should().Be("type");
    }

    [TestMethod]
    public void SwitchLabelsShouldBeParsedCorrectly()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Methods"":[{
                ""Name"":""Method"",
                ""Statements"":[{
                    ""$type"": ""LivingDocumentation.Switch, LivingDocumentation.Statements"",
                    ""Sections"":[{
                        ""Labels"": [""System.String""]
                    }]
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods[0].Statements[0].Should().BeOfType<Switch>();

        var @switch = (Switch)types[0].Methods[0].Statements[0];
        @switch.Sections[0].Labels.Should().HaveCount(1);
        @switch.Sections[0].Labels.Should().Contain("System.String");
    }

    [TestMethod]
    public void AStatementInASwitchSectionShouldHaveTheSwitchSectionAsParent()
    {
        // Assign
        var json = @"[{
            ""FullName"":""Test"",
            ""Methods"":[{
                ""Name"":""Method"",
                ""Statements"":[{
                    ""$type"": ""LivingDocumentation.Switch, LivingDocumentation.Statements"",
                    ""Sections"":[{
                        ""Statements"":[{
                            ""$type"":""LivingDocumentation.InvocationDescription, LivingDocumentation.Descriptions"",
                            ""ContainingType"": ""Test"",
                            ""Name"": ""TestMethod""
                        }]
                    }]
                }]
            }]
        }]";

        // Act
        var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

        // Assert
        types[0].Methods[0].Statements[0].Should().BeOfType<Switch>();

        var @switch = (Switch)types[0].Methods[0].Statements[0];
        @switch.Sections[0].Statements[0].Parent.Should().Be(@switch.Sections[0]);
    }
}
