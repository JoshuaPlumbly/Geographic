using UnityEngine;

public class ShapeGenerator
{
	ShapeSettings _settings;
	INosieFillter[] _noiseFilter;

	public MinMaxF ElevationMinMax { get; private set; }

	public void UpdateSettings(ShapeSettings settings)
	{
		_settings = settings;
		_noiseFilter = new INosieFillter[settings.noiseLayers.Length];

		for (int i = 0; i < _noiseFilter.Length; i++)
		{
			_noiseFilter[i] = NoiseFilterFactory.CreateNoiseFilter(_settings.noiseLayers[i].noiseSettings);
		}

		ElevationMinMax = new MinMaxF();
	}

	public Vector3 CalculatePointFrom(Vector3 pointOnUnitSphere)
	{
		float elevation = 0;

		float layerValue = 0;

		if (_noiseFilter.Length > 0)
		{
			layerValue = _noiseFilter[0].Evaluate(pointOnUnitSphere);

			if (_settings.noiseLayers[0].enabled)
				elevation = layerValue;
		}

		for (int i = 1; i < _noiseFilter.Length; i++)
		{
			if (!_settings.noiseLayers[i].enabled)
				continue;

			float mask = _settings.noiseLayers[i].useFirstLayerAsMask ? layerValue : 1;
			elevation += _noiseFilter[i].Evaluate(pointOnUnitSphere) * mask;
		}

		elevation = _settings.radius * (1 + elevation);
		ElevationMinMax.AddValue(elevation);

		return pointOnUnitSphere * elevation;
	}
}