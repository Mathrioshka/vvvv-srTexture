using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SlimDX.Direct3D11;
using VVVV.DX11;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes.SR
{
	[PluginInfo(Name = "R", Category = "DX11.Texture", Version = "2d", Author = "alg")]
	public class RNode : IPluginEvaluate, IDX11ResourceProvider
	{
		[Input("Receive String", IsSingle = true)]
		private ISpread<string> FReceiveStringIn;

		[Output("Output Texture")]
		private ISpread<DX11Resource<DX11Texture2D>> FTextureOut;

		private Texture2D FPrevTexture;
		private Texture2D FTexture;
		private bool FInvalidate;

		public void Evaluate(int spreadMax)
		{
			FPrevTexture = FTexture;

			if (FTextureOut[0] == null)
            {
                FTextureOut[0] = new DX11Resource<DX11Texture2D>();
            }

			var data = DataHolder.Instance;
			bool found;
			FTexture = data.GetData(FReceiveStringIn[0], out found);
			if(!found) return;

			if (FPrevTexture == null || FTexture.Description != FPrevTexture.Description)
			{
				FInvalidate = true;
			}
		}

	    public void Update(IPluginIO pin, DX11RenderContext context)
	    {
            if (FInvalidate)
            {
                if (FTexture == null) return;

                var texture = new Texture2D(context.Device, FTexture.Description);
                var shaderResourceView = new ShaderResourceView(context.Device, texture);
                var resource = DX11Texture2D.FromTextureAndSRV(context, texture, shaderResourceView);

                FTextureOut[0][context] = resource;

                FInvalidate = false;
            }

            if (FTexture == null) return;

            context.CurrentDeviceContext.CopyResource(FTexture, FTextureOut[0][context].Resource);
	    }

	    public void Destroy(IPluginIO pin, DX11RenderContext context, bool force)
	    {
	        throw new System.NotImplementedException();
	    }
	}
}
