using System.Collections.Generic;
using SlimDX.Direct3D11;

namespace VVVV.Nodes.SR
{
	public class DataHolder
	{
		private static DataHolder FInstance;

		public static DataHolder Instance
		{
			get { return FInstance ?? (FInstance = new DataHolder()); }
		}

		private readonly Dictionary<string, Texture2D> FData = new Dictionary<string, Texture2D>();

		public void RemoveData(string key)
		{
			if (!FData.ContainsKey(key)) return;

			FData.Remove(key);
		}

		public void UpdateData(string key, Texture2D texture)
		{
			if (!FData.ContainsKey(key))
			{
				FData.Add(key, texture);
			}

			FData[key] = texture;
		}

		public Texture2D GetData(string key, out bool found)
		{
			if (FData.ContainsKey(key))
			{
				found = true;
				return FData[key];
			}

			found = false;

			return null;
		}
	}
}
