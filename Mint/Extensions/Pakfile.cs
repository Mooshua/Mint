using System;
using System.IO;
using System.IO.Compression;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;

using LibBSP;

using Spectre.Console;

using ZipFile = ICSharpCode.SharpZipLib.Zip.ZipFile;

namespace Mint.Extensions;

public class Pakfile : ILump
{

		/// <summary>
		/// The <see cref="BSP"/> this <see cref="ILump"/> came from.
		/// </summary>
		public BSP Bsp { get; protected set; }

		/// <summary>
		/// The <see cref="LibBSP.LumpInfo"/> associated with this <see cref="ILump"/>.
		/// </summary>
		public LumpInfo LumpInfo { get; protected set; }
		
		public ZipFile Source { get; protected set; }
		
		protected MemoryStream Stream { get; set; }

		public int Length
		{
			get => (int) Stream.Length;
		}



		/// <summary>
		/// Parses the passed <c>byte</c> array into a <see cref="Pakfile"/> object.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Pakfile(byte[] data, BSP bsp, LumpInfo lumpInfo = default(LumpInfo)) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			bsp.Lumps[40] = this;
			
			Stream = new MemoryStream();
			Stream.Write(data, 0, data.Length);
			Stream.Seek(0, SeekOrigin.Begin);
			

			Source = new ZipFile(Stream);
			Bsp = bsp;
			LumpInfo = lumpInfo;

		}

		public ZipEntry? Get(string name)
		{
			return Source.GetEntry(name);
		}

		public void Create(IStaticDataSource source, ZipEntry entry)
		{
			Source.BeginUpdate();
			Source.Add(source, entry);
			Source.CommitUpdate();
		}

		public void Delete(ZipEntry entry)
		{
			Source.BeginUpdate();
			Source.Delete(entry);
			Source.CommitUpdate();
		}

		public Stream Read(ZipEntry entry)
		{
			//	Turns out InflateStream .Length's throw exceptions
			//	...yay?
			MemoryStream lengthSupporting = new MemoryStream();
			var inflate = Source.GetInputStream(entry);
			inflate.CopyTo(lengthSupporting);
			lengthSupporting.SetLength(lengthSupporting.Position);
			lengthSupporting.Seek(0, SeekOrigin.Begin);
			return lengthSupporting;
		}

		public class StreamSource : IStaticDataSource
		{
			public Stream Source { get; set; }

			public Stream GetSource() => Source;
		}

		public class PakfileDataSource : IStaticDataSource
		{
			public Pakfile Parent { get; init; }
			
			public ZipEntry Entry { get; init; }

			public Stream GetSource()
			{
				return Parent.Read(Entry);
			}
		}

		/// <summary>
		/// Gets all the data in this lump as a byte array.
		/// </summary>
		/// <returns>The data.</returns>
		public byte[] GetBytes()
		{
			/*var ms = new MemoryStream();
			var newZip = ZipFile.Create(ms);

			newZip.UseZip64 = UseZip64.Off;
			
			newZip.BeginUpdate();
			
			foreach (ZipEntry o in Source)
			{
				AnsiConsole.WriteLine(o.Name);
				o.CompressionMethod = CompressionMethod.Stored;
				Stream s = Read(o);
				o.Size = s.Length;
				//	Grr...
				newZip.Add(new StreamSource() { Source = s }, o);
			}
			newZip.CommitUpdate();
			
			*/
			File.WriteAllBytes("C:\\repo\\mint_test\\raw.zip", Stream.ToArray());

			return Stream.ToArray();
		}
}