using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    public NoiseSettings.RigidNoiseSettings settings;
    public Noise noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }


    public float Evaluate(Vector3 point)
    {
        /**
         * settings.centre is like an offset, it just moves it from up or down the grid, more flexibility on what you make.
         * settings.roughness is like zooming out from the noise, changes the frequecy/wavelengths
         *      --> https://www.redblobgames.com/maps/terrain-from-noise/#frequency
         *      --> exactly what happens when multiplying a one-dimensional sine wave or something.
         * settings.strengh is changing the amplitudes
         * if you imagine the generating noise values i.e [0,1, 0.25, 0.251, 0.234, 0.94, 0.123] and then plotting it
         * it'll perform some sort of frequency wave with max amplitude of 1 and min of 0.
         * 
         * Now we can better visualise what's really happening here.
         * 
         **/

        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        /**
         * Problem this for loop solves: Increasing the roughness ruins the shape of the sphere.
         * Solution: Adding noise at different roughness/frequencies but decreasing the amplitude at the same time.
         * i.e Each layer will have increased roughess and decreased amplitude than the previous.
         * 
         * For a flat map, this will make it a lot more "bumpy" llike a natural hilly look, not just very smooth.
         * 
         */
        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            v *= v; // Same as v^2 which is an exponential thing where the peaks will get more pontier.
            v *= Mathf.Clamp01(weight * settings.weightMultiplier);

            // We want to have the noise in the ridge more detailed than below, and we can do this (not the only way)
            // This makes sure that points that *start* off small will remain undetailed/small, and the points the start off
            // big will stay big and detailed.
            weight = v;

            noiseValue += v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        // if our noise value is smaller than our min value, than it should be set to 0

        //if (noiseValue < settings.minNoiseValue)
        //{
        //    noiseValue = 0;
        //} else
        //{
        //    noiseValue = noiseValue - settings.minNoiseValue;
        //}

        noiseValue = Mathf.Max(0, noiseValue - settings.minNoiseValue);
        return noiseValue * settings.strength;
    }
}
