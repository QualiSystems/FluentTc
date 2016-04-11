using System;
using System.Collections.Generic;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildParameterTypeBuilderFormatTests
    {
        [TestCase(null, null, "checkbox")]
        [TestCase(null, "kek", "checkbox uncheckedValue='kek'")]
        [TestCase("lol", null, "checkbox checkedValue='lol'")]
        [TestCase("lol", "kek", "checkbox checkedValue='lol' uncheckedValue='kek'")]
        public void BuildParameterCheckboxTypeBuilder_FormatTest(string checkedValue, string uncheckedValue, string expected)
        {
            // Arrange
            var testObject = new BuildParameterCheckboxTypeBuilder();
            if (checkedValue != null)
                testObject.WithCheckedValue(checkedValue);
            if (uncheckedValue != null)
                testObject.WithUncheckedValue(uncheckedValue);

            // Act
            var result = testObject.Build();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, null, null, "text validationMode='any'")]
        [TestCase(0, "kek", null, "text validationMode='any'")]
        [TestCase(0, null, "lol", "text validationMode='any'")]
        [TestCase(0, "kek", "lol", "text validationMode='any'")]
        [TestCase(1, null, null, "text validationMode='not_empty'")]
        [TestCase(1, "kek", null, "text validationMode='not_empty'")]
        [TestCase(1, null, "lol", "text validationMode='not_empty'")]
        [TestCase(1, "kek", "lol", "text validationMode='not_empty'")]
        [TestCase(2, null, null, "text validationMode='regex'")]
        [TestCase(2, "kek", null, "text validationMode='regex' regexp='kek'")]
        [TestCase(2, null, "lol", "text validationMode='regex' validationMessage='lol'")]
        [TestCase(2, "kek", "lol", "text validationMode='regex' regexp='kek' validationMessage='lol'")]
        public void BuildParameterTextTypeBuilder_FormatTest(int validation, string regexp, string validationMessage, string expected)
        {
            // Arrange
            var testObject = new BuildParameterTextTypeBuilder();
            var validationMap = new Dictionary<int, Action>
            {
                {0, () => testObject.AsAny()},
                {1, () => testObject.AsNotEmpty()},
                {2, () => testObject.AsRegex(regexp, validationMessage)}
            };
            if (!validationMap.ContainsKey(validation))
                Assert.Inconclusive("Wrong " + nameof(validation) + " argument value passed");
            validationMap[validation]();

            // Act
            var result = testObject.Build();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(null, null, null, null, null, "")]
        [TestCase(true, null, null, null, null, "")]
        [TestCase(false, null, null, null, null, "")]
        [TestCase(null, "||", null, null, null, "")]
        [TestCase(true, "||", null, null, null, "")]
        [TestCase(false, "||", null, null, null, "")]
        [TestCase(null, null, "uv", null, null, "select data_1='uv'")]
        [TestCase(true, null, "uv", null, null, "select multiple='true' data_1='uv'")]
        [TestCase(false, null, "uv", null, null, "select data_1='uv'")]
        [TestCase(null, "||", "uv", null, null, "select data_1='uv'")]
        [TestCase(true, "||", "uv", null, null, "select multiple='true' valueSeparator='||' data_1='uv'")]
        [TestCase(false, "||", "uv", null, null, "select data_1='uv'")]
        [TestCase(null, null, null, "l", null, "")]
        [TestCase(true, null, null, "l", null, "")]
        [TestCase(false, null, null, "l", null, "")]
        [TestCase(null, "||", null, "l", null, "")]
        [TestCase(true, "||", null, "l", null, "")]
        [TestCase(false, "||", null, "l", null, "")]
        [TestCase(null, null, "uv", "l", null, "select data_1='uv'")]
        [TestCase(true, null, "uv", "l", null, "select multiple='true' data_1='uv'")]
        [TestCase(false, null, "uv", "l", null, "select data_1='uv'")]
        [TestCase(null, "||", "uv", "l", null, "select data_1='uv'")]
        [TestCase(true, "||", "uv", "l", null, "select multiple='true' valueSeparator='||' data_1='uv'")]
        [TestCase(false, "||", "uv", "l", null, "select data_1='uv'")]
        [TestCase(null, null, null, null, "v", "select data_1='v'")]
        [TestCase(true, null, null, null, "v", "select multiple='true' data_1='v'")]
        [TestCase(false, null, null, null, "v", "select data_1='v'")]
        [TestCase(null, "||", null, null, "v", "select data_1='v'")]
        [TestCase(true, "||", null, null, "v", "select multiple='true' valueSeparator='||' data_1='v'")]
        [TestCase(false, "||", null, null, "v", "select data_1='v'")]
        [TestCase(null, null, "uv", null, "v", "select data_1='uv' data_2='v'")]
        [TestCase(true, null, "uv", null, "v", "select multiple='true' data_1='uv' data_2='v'")]
        [TestCase(false, null, "uv", null, "v", "select data_1='uv' data_2='v'")]
        [TestCase(null, "||", "uv", null, "v", "select data_1='uv' data_2='v'")]
        [TestCase(true, "||", "uv", null, "v", "select multiple='true' valueSeparator='||' data_1='uv' data_2='v'")]
        [TestCase(false, "||", "uv", null, "v", "select data_1='uv' data_2='v'")]
        [TestCase(null, null, null, "l", "v", "select label_1='l' data_1='v'")]
        [TestCase(true, null, null, "l", "v", "select multiple='true' label_1='l' data_1='v'")]
        [TestCase(false, null, null, "l", "v", "select label_1='l' data_1='v'")]
        [TestCase(null, "||", null, "l", "v", "select label_1='l' data_1='v'")]
        [TestCase(true, "||", null, "l", "v", "select multiple='true' valueSeparator='||' label_1='l' data_1='v'")]
        [TestCase(false, "||", null, "l", "v", "select label_1='l' data_1='v'")]
        [TestCase(null, null, "uv", "l", "v", "select data_1='uv' label_2='l' data_2='v'")]
        [TestCase(true, null, "uv", "l", "v", "select multiple='true' data_1='uv' label_2='l' data_2='v'")]
        [TestCase(false, null, "uv", "l", "v", "select data_1='uv' label_2='l' data_2='v'")]
        [TestCase(null, "||", "uv", "l", "v", "select data_1='uv' label_2='l' data_2='v'")]
        [TestCase(true, "||", "uv", "l", "v", "select multiple='true' valueSeparator='||' data_1='uv' label_2='l' data_2='v'")]
        [TestCase(false, "||", "uv", "l", "v", "select data_1='uv' label_2='l' data_2='v'")]
        public void BuildParameterSelectListTypeBuilder_FormatTest(bool? allowMultiple, string separator, string unlabeledValue, string label, string value, string expected)
        {
            // Arrange
            var testObject = new BuildParameterSelectListTypeBuilder();

            if (allowMultiple != null)
                testObject.AllowMultiple(allowMultiple.Value);
            if (separator != null)
                testObject.WithSeparator(separator);
            if (unlabeledValue != null)
                testObject.Value(unlabeledValue);
            if (value != null)
                testObject.LabeledValue(label, value);

            // Act
            var result = testObject.Build();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, 0, null, null, "")]
        [TestCase(1, 0, null, null, "password display='normal'")]
        [TestCase(0, 1, null, null, "")]
        [TestCase(1, 1, null, null, "password display='normal'")]
        [TestCase(0, 2, null, null, "")]
        [TestCase(1, 2, null, null, "password display='prompt'")]
        [TestCase(0, 3, null, null, "")]
        [TestCase(1, 3, null, null, "password display='hidden'")]
        [TestCase(0, 0, "lol", null, "")]
        [TestCase(1, 0, "lol", null, "password label='lol' display='normal'")]
        [TestCase(0, 1, "lol", null, "")]
        [TestCase(1, 1, "lol", null, "password label='lol' display='normal'")]
        [TestCase(0, 2, "lol", null, "")]
        [TestCase(1, 2, "lol", null, "password label='lol' display='prompt'")]
        [TestCase(0, 3, "lol", null, "")]
        [TestCase(1, 3, "lol", null, "password label='lol' display='hidden'")]
        [TestCase(0, 0, null, "kek", "")]
        [TestCase(1, 0, null, "kek", "password description='kek' display='normal'")]
        [TestCase(0, 1, null, "kek", "")]
        [TestCase(1, 1, null, "kek", "password description='kek' display='normal'")]
        [TestCase(0, 2, null, "kek", "")]
        [TestCase(1, 2, null, "kek", "password description='kek' display='prompt'")]
        [TestCase(0, 3, null, "kek", "")]
        [TestCase(1, 3, null, "kek", "password description='kek' display='hidden'")]
        [TestCase(0, 0, "lol", "kek", "")]
        [TestCase(1, 0, "lol", "kek", "password label='lol' description='kek' display='normal'")]
        [TestCase(0, 1, "lol", "kek", "")]
        [TestCase(1, 1, "lol", "kek", "password label='lol' description='kek' display='normal'")]
        [TestCase(0, 2, "lol", "kek", "")]
        [TestCase(1, 2, "lol", "kek", "password label='lol' description='kek' display='prompt'")]
        [TestCase(0, 3, "lol", "kek", "")]
        [TestCase(1, 3, "lol", "kek", "password label='lol' description='kek' display='hidden'")]
        public void BuildParameterTypeBuilder_FormatTest(int type, int display, string label, string description, string expected)
        {
            // Arrange
            var testObject = new BuildParameterTypeBuilder();
            var typeMap = new Dictionary<int, Action>
            {
                {0, () => { }},
                {1, () => testObject.AsPassword()}
            };
            if (!typeMap.ContainsKey(type))
                Assert.Inconclusive("Wrong " + nameof(type) + " argument value passed");
            typeMap[type]();
            var displayMap = new Dictionary<int, Action>
            {
                {0, () => { }},
                {1, () => testObject.WithDisplayNormal()},
                {2, () => testObject.WithDisplayPrompt()},
                {3, () => testObject.WithDisplayHidden()}
            };
            if (!displayMap.ContainsKey(display))
                Assert.Inconclusive("Wrong " + nameof(display) + " argument value passed");
            displayMap[display]();
            if (label != null)
                testObject.WithLabel(label);
            if (description != null)
                testObject.WithDescription(description);

            // Act
            var result = testObject.Build();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
