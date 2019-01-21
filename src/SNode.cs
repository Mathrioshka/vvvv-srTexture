using System.ComponentModel.Composition;
using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SlimDX.Direct3D11;
using VVVV.DX11;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes.SR
{
	[PluginInfo(Name = "S", Category = "DX11.Texture", Version = "2d", Author = "alg", AutoEvaluate = true)]
	public class SNode : IPluginEvaluate, IDX11ResourceDataRetriever
	{
		[Input("Input Texture")]
		private Pin<DX11Resource<DX11Texture2D>> FTextureIn;

		[Input("Send String", DefaultString = "devnull", IsSingle = true)] 
		private ISpread<string> FSendStringIn;

		[Import]
		IPluginHost FHost;

		private Texture2D FPrevTexture;

		public void Evaluate(int spreadMax)
		{
			var data = DataHolder.Instance;
			data.UpdateData(FSendStringIn[0], FPrevTexture);

			if (FTextureIn.PluginIO.IsConnected)
			{
				if (RenderRequest != null) { RenderRequest(this, FHost); }
				
				if(AssignedContext == null) return;

				var dx11Texture = FTextureIn[0][AssignedContext];
				if(dx11Texture == null) return;

				var texture2D = FTextureIn[0][AssignedContext].Resource;
				if(FPrevTexture == null || FPrevTexture.Description != texture2D.Description)
				FPrevTexture = new Texture2D(AssignedContext.Device, texture2D.Description);
				AssignedContext.CurrentDeviceContext.CopyResource(texture2D, FPrevTexture);
			}
			else
			{
				FPrevTexture = null;
			}
		}

		public DX11RenderContext AssignedContext { get; set; }
		public event DX11RenderRequestDelegate RenderRequest;
	}
}
