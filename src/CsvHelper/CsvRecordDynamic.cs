using System;
using System.Collections.Generic;
using System.Dynamic;
using CsvHelper.Configuration;

namespace CsvHelper
{
	public class CsvRecordDynamic : DynamicObject
	{
		private readonly CsvConfiguration configuration;
		private readonly Dictionary<string, List<int>> namedIndexes;
		private readonly string[] currentRecord;

		public CsvRecordDynamic( CsvConfiguration configuration, Dictionary<string, List<int>> namedIndexes, string[] currentRecord )
		{
			if( configuration == null )
			{
				throw new ArgumentNullException( "configuration" );
			}
			if( namedIndexes == null )
			{
				throw new ArgumentNullException( "namedIndexes" );
			}
			if( currentRecord == null )
			{
				throw new ArgumentNullException( "currentRecord" );
			}

			this.configuration = configuration;
			this.namedIndexes = namedIndexes;
			this.currentRecord = currentRecord;
		}

		public override bool TryGetMember( GetMemberBinder binder, out object result )
		{
			var index = GetFieldIndex( binder.Name );
			result = index == -1 ? null : this.currentRecord[index];
			if( index == -1 && configuration.IsStrictMode )
			{
				// If the index isn't found and we're in strict mode
				// the get member should fail. If we're not in strict
				// mode, the object is just set to null.
				return false;
			}
			return true;
		}

		/// <summary>
		/// Gets the index of the field at name if found.
		/// </summary>
		/// <param name="name">The name of the field to get the index for.</param>
		/// <param name="index">The index of the field if there are multiple fields with the same name.</param>
		/// <returns>The index of the field if found, otherwise -1.</returns>
		/// <exception cref="CsvReaderException">Thrown if there is no header record.</exception>
		/// <exception cref="CsvMissingFieldException">Thrown if there isn't a field with name.</exception>
		protected virtual int GetFieldIndex( string name, int index = 0 )
		{
			if( !configuration.HasHeaderRecord )
			{
				throw new CsvReaderException( "There is no header record to determine the index by name." );
			}

			if( !configuration.IsCaseSensitive )
			{
				name = name.ToLower();
			}

			if( !namedIndexes.ContainsKey( name ) )
			{
				return -1;
			}

			return namedIndexes[name][index];
		}
	}
}
