﻿#if NET5_0 || NET45 || NET46 || NET47 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1

using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Sheleski.DelimitedFile
{
	partial class CsvFile
	{
		public Task WriteAsync(TextWriter writer)
		{
			return WriteAsync(writer, CsvFileOptions.WithHeaders, CancellationToken.None);
		}

		public Task WriteAsync(TextWriter writer, CsvFileOptions options)
		{
			return WriteAsync(writer, options, CancellationToken.None);
		}

		public Task WriteAsync(TextWriter writer, CancellationToken cancellationToken)
		{
			return WriteAsync(writer, CsvFileOptions.WithHeaders, cancellationToken);
		}

		public async Task WriteAsync(TextWriter writer, CsvFileOptions options, CancellationToken cancellationToken)
		{
			await DelimitedFile.WriteAsync(this, writer, options, cancellationToken);
		}

		public Task SaveAsync(string filePath)
		{
			return SaveAsync(filePath, CancellationToken.None);
		}

		public Task SaveAsync(string filePath, CancellationToken cancellationToken)
		{
			return SaveAsync(filePath, CsvFileOptions.WithHeaders, cancellationToken);
		}

		public Task SaveAsync(string filePath, CsvFileOptions options)
		{
			return SaveAsync(filePath, options, CancellationToken.None);
		}

		public async Task SaveAsync(string filePath, CsvFileOptions options, CancellationToken cancellationToken)
		{
			using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
			using (var writer = new StreamWriter(stream))
			{
				await WriteAsync(writer, options, cancellationToken);
			}
		}
	}
}

#endif