float4x4 World;
float4x4 View;
float4x4 Projection;
texture terrainTexture1;
sampler2D textureSampler = sampler_state {
	Texture = (terrainTexture1);
	AddressU = Wrap;
	AddressV = Wrap;
};

// TODO: add effect parameters here.

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

		output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return tex2D(textureSampler, input.TextureCoordinate);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
