public static class NoiseFilterFactory
{
    public static INosieFillter CreateNoiseFilter(NoiseSettings settings)
    {
        switch(settings.filterType)
        {
            case NoiseSettings.FilterType.Simple:
                return new NoiseFilter(settings);
            case NoiseSettings.FilterType.Ridgid:
                return new RidgidNoiseFilter(settings);
        }

        return null;
    }
}