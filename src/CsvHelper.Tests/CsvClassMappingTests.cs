﻿#region License
// Copyright 2009-2011 Josh Close
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com
#endregion
using System;
using System.ComponentModel;
using CsvHelper.Configuration;
using Xunit;

namespace CsvHelper.Tests
{
	public class CsvClassMappingTests
	{
		[Fact]
		public void MapTest()
		{
			var map = new TestMappingDefaultClass();

			Assert.Equal( 3, map.PropertyMaps.Count );

			Assert.Equal( "GuidColumn", map.PropertyMaps[0].NameValue );
			Assert.Equal( -1, map.PropertyMaps[0].IndexValue );
			Assert.Equal( typeof( GuidConverter ), map.PropertyMaps[0].TypeConverterValue.GetType() );

			Assert.Equal( "IntColumn", map.PropertyMaps[1].NameValue );
			Assert.Equal( -1, map.PropertyMaps[1].IndexValue );
			Assert.Equal( typeof( Int32Converter ), map.PropertyMaps[1].TypeConverterValue.GetType() );

			Assert.Equal( "StringColumn", map.PropertyMaps[2].NameValue );
			Assert.Equal( -1, map.PropertyMaps[2].IndexValue );
			Assert.Equal( typeof( StringConverter ), map.PropertyMaps[2].TypeConverterValue.GetType() );
		}

		[Fact]
		public void MapNameTest()
		{
			var map = new TestMappingNameClass();

			Assert.Equal( 3, map.PropertyMaps.Count );

			Assert.Equal( "Guid Column", map.PropertyMaps[0].NameValue );
			Assert.Equal( "Int Column", map.PropertyMaps[1].NameValue );
			Assert.Equal( "String Column", map.PropertyMaps[2].NameValue );
		}

		[Fact]
		public void MapIndexTest()
		{
			var map = new TestMappingIndexClass();

			Assert.Equal( 3, map.PropertyMaps.Count );

			Assert.Equal( 2, map.PropertyMaps[0].IndexValue );
			Assert.Equal( 3, map.PropertyMaps[1].IndexValue );
			Assert.Equal( 1, map.PropertyMaps[2].IndexValue );
		}

		[Fact]
		public void MapIgnoreTest()
		{
			var map = new TestMappingIngoreClass();

			Assert.Equal( 3, map.PropertyMaps.Count );

			Assert.True( map.PropertyMaps[0].IgnoreValue );
			Assert.False( map.PropertyMaps[1].IgnoreValue );
			Assert.True( map.PropertyMaps[2].IgnoreValue );
		}

		[Fact]
		public void MapTypeConverterTest()
		{
			var map = new TestMappingTypeConverterClass();

			Assert.Equal( 3, map.PropertyMaps.Count );

			Assert.IsType<Int16Converter>( map.PropertyMaps[0].TypeConverterValue );
			Assert.IsType<StringConverter>( map.PropertyMaps[1].TypeConverterValue );
			Assert.IsType<Int64Converter>( map.PropertyMaps[2].TypeConverterValue );
		}

		[Fact]
		public void MapMultipleNamesTest()
		{
			var map = new TestMappingMultipleNamesClass();

			Assert.Equal( 3, map.PropertyMaps.Count );

			Assert.Equal( 3, map.PropertyMaps[0].NamesValue.Length );
			Assert.Equal( 3, map.PropertyMaps[1].NamesValue.Length );
			Assert.Equal( 3, map.PropertyMaps[2].NamesValue.Length );

			Assert.Equal( "guid1", map.PropertyMaps[0].NamesValue[0] );
			Assert.Equal( "guid2", map.PropertyMaps[0].NamesValue[1] );
			Assert.Equal( "guid3", map.PropertyMaps[0].NamesValue[2] );

			Assert.Equal( "int1", map.PropertyMaps[1].NamesValue[0] );
			Assert.Equal( "int2", map.PropertyMaps[1].NamesValue[1] );
			Assert.Equal( "int3", map.PropertyMaps[1].NamesValue[2] );

			Assert.Equal( "string1", map.PropertyMaps[2].NamesValue[0] );
			Assert.Equal( "string2", map.PropertyMaps[2].NamesValue[1] );
			Assert.Equal( "string3", map.PropertyMaps[2].NamesValue[2] );
		}

		[Fact]
		public void MapConstructorTest()
		{
			var map = new TestMappingConstructorClass();

			Assert.NotNull( map.Constructor );
		}

		private class TestClass
		{
			public string StringColumn { get; set; }
			public int IntColumn { get; set; }
			public Guid GuidColumn { get; set; }
			public string NotUsedColumn { get; set; }

			public TestClass(){}

			public TestClass( string stringColumn )
			{
				StringColumn = stringColumn;
			}
		}
		
		private sealed class TestMappingConstructorClass : CsvClassMap<TestClass>
		{
			public TestMappingConstructorClass()
			{
				ConstructUsing( () => new TestClass( "String Column" ) );
			}
		}

		private sealed class TestMappingDefaultClass : CsvClassMap<TestClass>
		{
			public TestMappingDefaultClass()
			{
				Map( m => m.GuidColumn );
				Map( m => m.IntColumn );
				Map( m => m.StringColumn );
			}
		}

		private sealed class TestMappingNameClass : CsvClassMap<TestClass>
		{
			public TestMappingNameClass()
			{
				Map( m => m.GuidColumn ).Name( "Guid Column" );
				Map( m => m.IntColumn ).Name( "Int Column" );
				Map( m => m.StringColumn ).Name( "String Column" );
			}
		}

		private sealed class TestMappingIndexClass : CsvClassMap<TestClass>
		{
			public TestMappingIndexClass()
			{
				Map( m => m.GuidColumn ).Index( 3 );
				Map( m => m.IntColumn ).Index( 2 );
				Map( m => m.StringColumn ).Index( 1 );
			}
		}

		private sealed class TestMappingIngoreClass : CsvClassMap<TestClass>
		{
			public TestMappingIngoreClass()
			{
				Map( m => m.GuidColumn ).Ignore();
				Map( m => m.IntColumn );
				Map( m => m.StringColumn ).Ignore();
			}
		}

		private sealed class TestMappingTypeConverterClass : CsvClassMap<TestClass>
		{
			public TestMappingTypeConverterClass()
			{
				Map( m => m.GuidColumn ).TypeConverter<Int16Converter>();
				Map( m => m.IntColumn ).TypeConverter<StringConverter>();
				Map( m => m.StringColumn ).TypeConverter( new Int64Converter() );
			}
		}

		private sealed class TestMappingMultipleNamesClass : CsvClassMap<TestClass>
		{
			public TestMappingMultipleNamesClass()
			{
				Map( m => m.GuidColumn ).Name( "guid1", "guid2", "guid3" );
				Map( m => m.IntColumn ).Name( "int1", "int2", "int3" );
				Map( m => m.StringColumn ).Name( "string1", "string2", "string3" );
			}
		}
	}
}
