using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{

    ShapeSettings shapeSettings;
    INoiseFilter[] noiseFilters;

    public ShapeGenerator(ShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
        this.noiseFilters = new INoiseFilter[shapeSettings.noiseLayers.Length];

        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(shapeSettings.noiseLayers[i].noiseSettings);
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (shapeSettings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (shapeSettings.noiseLayers[i].enabled)
            {
                // this works well with the line in NoiseFilter.cs: noiseValue = Mathf.Max(0, noiseValue - settings.minNoiseValue);
                // as depending on the settings.minNoiseValue for different filters, this Evaluate function can return 0, meaning
                // some noise filters wont affect some points, but others will. This creates that "distinction" or "layers" that we
                // see.
                // From what I can tell without that Mathf.Max() line, there's no real point in layer filters like this.
                // Since we'll just be doing .. planetRadius * (1 + elevation) where elevation is just a sum --
                // Which is basically just a bigger number, meaning what we return is a bigger number. Which can be achieved with one filter.


                // The mask also works because of the Mathf.Max(0...) logic.
                // Where the first layer is 0, the other layers using this as a "mask" will also be 0 because multiply anything with 0 is 0.
                // So when making changes with these layers using the first layer as masks, it will only make changes to the ones where
                // the first layer has a elevation greater than 0.
                
                float mask = shapeSettings.noiseLayers[i].useFirstLayerAsMask ? firstLayerValue : 1f;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }   

        // Why add 1 to the elevation? Perhaps because elevation is [0,1], having a value of 0 makes the point onf the sphere 0.
        // And it doesn't make sense to return a pointonunitsphere with value 0 hehe.
        return pointOnUnitSphere * shapeSettings.planetRadius * (1 + elevation);  
    }

}
