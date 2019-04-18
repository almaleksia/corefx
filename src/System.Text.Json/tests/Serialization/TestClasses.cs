﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public interface ITestClass
    {
        void Initialize();
        void Verify();
    }

    public enum SampleEnum
    {
        One = 1,
        Two = 2
    }

    public class TestClassWithNull
    {
        public string MyString { get; set; }
        public static readonly string s_json =
                @"{" +
                @"""MyString"" : null" +
                @"}";

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(s_json);

        public void Initialize()
        {
            MyString = null;
        }

        public void Verify()
        {
            Assert.Equal(MyString, null);
        }
    }

    public class TestClassWithInitializedProperties
    {
        public string MyString { get; set; } = "Hello";
        public int? MyInt { get; set; } = 1;
        public static readonly string s_null_json =
                @"{" +
                @"""MyString"" : null," +
                @"""MyInt"" : null" +
                @"}";

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(s_null_json);
    }

    public class TestClassWithNestedObjectInner : ITestClass
    {
        public SimpleTestClass MyData { get; set; }

        public static readonly string s_json =
            @"{" +
                @"""MyData"":" + SimpleTestClass.s_json +
            @"}";

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(s_json);

        public void Initialize()
        {
            MyData = new SimpleTestClass();
            MyData.Initialize();
        }

        public void Verify()
        {
            Assert.NotNull(MyData);
            MyData.Verify();
        }
    }

    public class TestClassWithNestedObjectOuter : ITestClass
    {
        public TestClassWithNestedObjectInner MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":" + TestClassWithNestedObjectInner.s_json +
            @"}");

        public void Initialize()
        {
            MyData = new TestClassWithNestedObjectInner();
            MyData.Initialize();
        }

        public void Verify()
        {
            Assert.NotNull(MyData);
            MyData.Verify();
        }
    }

    public class TestClassWithObjectList : ITestClass
    {
        public List<SimpleTestClass> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    SimpleTestClass.s_json + "," +
                    SimpleTestClass.s_json +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<SimpleTestClass>();

            {
                SimpleTestClass obj = new SimpleTestClass();
                obj.Initialize();
                MyData.Add(obj);
            }

            {
                SimpleTestClass obj = new SimpleTestClass();
                obj.Initialize();
                MyData.Add(obj);
            }
        }

        public void Verify()
        {
            Assert.Equal(2, MyData.Count);
            MyData[0].Verify();
            MyData[1].Verify();
        }
    }

    public class TestClassWithObjectArray : ITestClass
    {
        public SimpleTestClass[] MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    SimpleTestClass.s_json + "," +
                    SimpleTestClass.s_json +
                @"]" +
            @"}");

        public void Initialize()
        {
            SimpleTestClass obj1 = new SimpleTestClass();
            obj1.Initialize();

            SimpleTestClass obj2 = new SimpleTestClass();
            obj2.Initialize();

            MyData = new SimpleTestClass[2] { obj1, obj2 };
        }

        public void Verify()
        {
            MyData[0].Verify();
            MyData[1].Verify();
            Assert.Equal(2, MyData.Length);
        }
    }

    public class TestClassWithObjectIEnumerableT : ITestClass
    {
        public IEnumerable<SimpleTestClass> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    SimpleTestClass.s_json + "," +
                    SimpleTestClass.s_json +
                @"]" +
            @"}");

        public void Initialize()
        {
            SimpleTestClass obj1 = new SimpleTestClass();
            obj1.Initialize();

            SimpleTestClass obj2 = new SimpleTestClass();
            obj2.Initialize();

            MyData = new SimpleTestClass[] { obj1, obj2 };
        }

        public void Verify()
        {
            int count = 0;

            foreach (SimpleTestClass data in MyData)
            {
                data.Verify();
                count++;
            }

            Assert.Equal(2, count);
        }
    }

    public class TestClassWithObjectIListT : ITestClass
    {
        public IList<SimpleTestClass> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    SimpleTestClass.s_json + "," +
                    SimpleTestClass.s_json +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<SimpleTestClass>();

            {
                SimpleTestClass obj = new SimpleTestClass();
                obj.Initialize();
                MyData.Add(obj);
            }

            {
                SimpleTestClass obj = new SimpleTestClass();
                obj.Initialize();
                MyData.Add(obj);
            }
        }

        public void Verify()
        {
            Assert.Equal(2, MyData.Count);
            MyData[0].Verify();
            MyData[1].Verify();
        }
    }

    public class TestClassWithObjectICollectionT : ITestClass
    {
        public ICollection<SimpleTestClass> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    SimpleTestClass.s_json + "," +
                    SimpleTestClass.s_json +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<SimpleTestClass>();

            {
                SimpleTestClass obj = new SimpleTestClass();
                obj.Initialize();
                MyData.Add(obj);
            }

            {
                SimpleTestClass obj = new SimpleTestClass();
                obj.Initialize();
                MyData.Add(obj);
            }
        }

        public void Verify()
        {
            Assert.Equal(2, MyData.Count);

            foreach (SimpleTestClass data in MyData)
            {
                data.Verify();
            }
        }
    }

    public class TestClassWithObjectIReadOnlyCollectionT : ITestClass
    {
        public IReadOnlyCollection<SimpleTestClass> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    SimpleTestClass.s_json + "," +
                    SimpleTestClass.s_json +
                @"]" +
            @"}");

        public void Initialize()
        {
            SimpleTestClass obj1 = new SimpleTestClass();
            obj1.Initialize();

            SimpleTestClass obj2 = new SimpleTestClass();
            obj2.Initialize();

            MyData = new SimpleTestClass[] { obj1, obj2 };
        }

        public void Verify()
        {
            Assert.Equal(2, MyData.Count);

            foreach (SimpleTestClass data in MyData)
            {
                data.Verify();
            }
        }
    }

    public class TestClassWithObjectIReadOnlyListT : ITestClass
    {
        public IReadOnlyList<SimpleTestClass> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    SimpleTestClass.s_json + "," +
                    SimpleTestClass.s_json +
                @"]" +
            @"}");

        public void Initialize()
        {
            SimpleTestClass obj1 = new SimpleTestClass();
            obj1.Initialize();

            SimpleTestClass obj2 = new SimpleTestClass();
            obj2.Initialize();

            MyData = new SimpleTestClass[] { obj1, obj2 };
        }

        public void Verify()
        {
            Assert.Equal(2, MyData.Count);
            MyData[0].Verify();
            MyData[1].Verify();
        }
    }

    public class TestClassWithStringArray : ITestClass
    {
        public string[] MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    @"""Hello""," +
                    @"""World""" +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new string[] { "Hello", "World" };
        }

        public void Verify()
        {
            Assert.Equal("Hello", MyData[0]);
            Assert.Equal("World", MyData[1]);
            Assert.Equal(2, MyData.Length);
        }
    }

    public class TestClassWithCycle
    {
        public TestClassWithCycle Parent { get; set; }

        public void Initialize()
        {
            Parent = this;
        }
    }

    public class TestClassWithGenericList : ITestClass
    {
        public List<string> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    @"""Hello""," +
                    @"""World""" +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<string>
            {
                "Hello",
                "World"
            };
            Assert.Equal(2, MyData.Count);
        }

        public void Verify()
        {
            Assert.Equal("Hello", MyData[0]);
            Assert.Equal("World", MyData[1]);
            Assert.Equal(2, MyData.Count);
        }
    }

    public class TestClassWithGenericIEnumerableT : ITestClass
    {
        public IEnumerable<string> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    @"""Hello""," +
                    @"""World""" +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<string>
            {
                "Hello",
                "World"
            };

            int count = 0;
            foreach (string data in MyData)
            {
                count++;
            }
            Assert.Equal(2, count);
        }

        public void Verify()
        {
            string[] expected = { "Hello", "World" };
            int count = 0;

            foreach (string data in MyData)
            {
                Assert.Equal(expected[count], data);
                count++;
            }

            Assert.Equal(2, count);
        }
    }

    public class TestClassWithGenericIListT : ITestClass
    {
        public IList<string> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    @"""Hello""," +
                    @"""World""" +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<string>
            {
                "Hello",
                "World"
            };
            Assert.Equal(2, MyData.Count);
        }

        public void Verify()
        {
            Assert.Equal("Hello", MyData[0]);
            Assert.Equal("World", MyData[1]);
            Assert.Equal(2, MyData.Count);
        }
    }

    public class TestClassWithGenericICollectionT : ITestClass
    {
        public ICollection<string> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    @"""Hello""," +
                    @"""World""" +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<string>
            {
                "Hello",
                "World"
            };
            Assert.Equal(2, MyData.Count);
        }

        public void Verify()
        {
            string[] expected = { "Hello", "World" };
            int i = 0;

            foreach (string data in MyData)
            {
                Assert.Equal(expected[i++], data);
            }

            Assert.Equal(2, MyData.Count);
        }
    }

    public class TestClassWithGenericIReadOnlyCollectionT : ITestClass
    {
        public IReadOnlyCollection<string> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    @"""Hello""," +
                    @"""World""" +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<string>
            {
                "Hello",
                "World"
            };
            Assert.Equal(2, MyData.Count);
        }

        public void Verify()
        {
            string[] expected = { "Hello", "World" };
            int i = 0;

            foreach (string data in MyData)
            {
                Assert.Equal(expected[i++], data);
            }

            Assert.Equal(2, MyData.Count);
        }
    }

    public class TestClassWithGenericIReadOnlyListT : ITestClass
    {
        public IReadOnlyList<string> MyData { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
                @"""MyData"":[" +
                    @"""Hello""," +
                    @"""World""" +
                @"]" +
            @"}");

        public void Initialize()
        {
            MyData = new List<string>
            {
                "Hello",
                "World"
            };
            Assert.Equal(2, MyData.Count);
        }

        public void Verify()
        {
            Assert.Equal("Hello", MyData[0]);
            Assert.Equal("World", MyData[1]);
            Assert.Equal(2, MyData.Count);
        }
    }

    public class SimpleDerivedTestClass : SimpleTestClass
    {
    }

    public class OverridePropertyNameRuntime_TestClass
    {
        public Int16 MyInt16 { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            @"{" +
            @"""blah"" : 1" +
            @"}"
        );
    }

    public class LargeDataTestClass : ITestClass
    {
        public List<LargeDataChildTestClass> Children { get; set; } = new List<LargeDataChildTestClass>();
        public const int ChildrenCount = 10;

        public string MyString { get; set; }
        public const int MyStringLength = 1000;

        public void Initialize()
        {
            MyString = new string('1', MyStringLength);

            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = new LargeDataChildTestClass
                {
                    MyString = new string('2', LargeDataChildTestClass.MyStringLength),
                    MyStringArray = new string[LargeDataChildTestClass.MyStringArrayArrayCount]
                };
                for (int j = 0; j < child.MyStringArray.Length; j++)
                {
                    child.MyStringArray[j] = new string('3', LargeDataChildTestClass.MyStringArrayElementStringLength);
                }

                Children.Add(child);
            }
        }

        public void Verify()
        {
            Assert.Equal(MyStringLength, MyString.Length);
            Assert.Equal('1', MyString[0]);
            Assert.Equal('1', MyString[MyStringLength - 1]);

            Assert.Equal(ChildrenCount, Children.Count);
            for (int i = 0; i < ChildrenCount; i++)
            {
                LargeDataChildTestClass child = Children[i];
                Assert.Equal(LargeDataChildTestClass.MyStringLength, child.MyString.Length);
                Assert.Equal('2', child.MyString[0]);
                Assert.Equal('2', child.MyString[LargeDataChildTestClass.MyStringLength - 1]);

                Assert.Equal(LargeDataChildTestClass.MyStringArrayArrayCount, child.MyStringArray.Length);
                for (int j = 0; j < LargeDataChildTestClass.MyStringArrayArrayCount; j++)
                {
                    Assert.Equal('3', child.MyStringArray[j][0]);
                    Assert.Equal('3', child.MyStringArray[j][LargeDataChildTestClass.MyStringArrayElementStringLength - 1]);
                }
            }
        }
    }

    public class LargeDataChildTestClass
    {
        public const int MyStringLength = 2000;
        public string MyString { get; set; }

        public string[] MyStringArray { get; set; }
        public const int MyStringArrayArrayCount = 1000;
        public const int MyStringArrayElementStringLength = 50;
    }

    public class EmptyClass { }

    public class BasicPerson : ITestClass
    {
        public int age { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public List<string> phoneNumbers { get; set; }
        public BasicJsonAddress address { get; set; }

        public void Initialize()
        {
            age = 30;
            first = "John";
            last = "Smith";
            phoneNumbers = new List<string> { "425-000-0000", "425-000-0001" };
            address = new BasicJsonAddress
            {
                street = "1 Microsoft Way",
                city = "Redmond",
                zip = 98052
            };
        }

        public void Verify()
        {
            Assert.Equal(30, age);
            Assert.Equal("John", first);
            Assert.Equal("Smith", last);
            Assert.Equal("425-000-0000", phoneNumbers[0]);
            Assert.Equal("425-000-0001", phoneNumbers[1]);
            Assert.Equal("1 Microsoft Way", address.street);
            Assert.Equal("Redmond", address.city);
            Assert.Equal(98052, address.zip);
        }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            "{" +
                @"""age"" : 30," +
                @"""first"" : ""John""," +
                @"""last"" : ""Smith""," +
                @"""phoneNumbers"" : [" +
                    @"""425-000-0000""," +
                    @"""425-000-0001""" +
                @"]," +
                @"""address"" : {" +
                    @"""street"" : ""1 Microsoft Way""," +
                    @"""city"" : ""Redmond""," +
                    @"""zip"" : 98052" +
                "}" +
            "}");
    }

    public class BasicJsonAddress
    {
        public string street { get; set; }
        public string city { get; set; }
        public int zip { get; set; }
    }

    public class BasicCompany : ITestClass
    {
        public List<BasicJsonAddress> sites { get; set; }
        public BasicJsonAddress mainSite { get; set; }
        public string name { get; set; }

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
            "{" +
                @"""name"" : ""Microsoft""," +
                @"""sites"" : [" +
                    "{" +
                        @"""street"" : ""1 Lone Tree Rd S""," +
                        @"""city"" : ""Fargo""," +
                        @"""zip"" : 58104" +
                    "}," +
                    "{" +
                        @"""street"" : ""8055 Microsoft Way""," +
                        @"""city"" : ""Charlotte""," +
                        @"""zip"" : 28273" +
                    "}" +
                @"]," +
                @"""mainSite"" : " +
                    "{" +
                        @"""street"" : ""1 Microsoft Way""," +
                        @"""city"" : ""Redmond""," +
                        @"""zip"" : 98052" +
                    "}" +
            "}");

        public void Initialize()
        {
            name = "Microsoft";
            sites = new List<BasicJsonAddress>
            {
                new BasicJsonAddress
                {
                    street = "1 Lone Tree Rd S",
                    city = "Fargo",
                    zip = 58104
                },
                new BasicJsonAddress
                {
                    street = "8055 Microsoft Way",
                    city = "Charlotte",
                    zip = 28273
                }
            };

            mainSite =
                new BasicJsonAddress
                {
                    street = "1 Microsoft Way",
                    city = "Redmond",
                    zip = 98052
                };
        }

        public void Verify()
        {
            Assert.Equal("Microsoft", name);
            Assert.Equal("1 Lone Tree Rd S", sites[0].street);
            Assert.Equal("Fargo", sites[0].city);
            Assert.Equal(58104, sites[0].zip);
            Assert.Equal("8055 Microsoft Way", sites[1].street);
            Assert.Equal("Charlotte", sites[1].city);
            Assert.Equal(28273, sites[1].zip);
            Assert.Equal("1 Microsoft Way", mainSite.street);
            Assert.Equal("Redmond", mainSite.city);
            Assert.Equal(98052, mainSite.zip);
        }
    }
}