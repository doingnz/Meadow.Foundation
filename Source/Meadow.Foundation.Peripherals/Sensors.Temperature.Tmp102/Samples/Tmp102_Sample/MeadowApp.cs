﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Temperature;
using System;
using System.Threading.Tasks;

namespace Sensors.Temperature.Tmp102_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        //<!=SNIP=>

        Tmp102 tmp102;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            tmp102 = new Tmp102(Device.CreateI2cBus());

            var consumer = Tmp102.CreateObserver(
                handler: result =>
                {
                    Resolver.Log.Info($"Temperature New Value {result.New.Celsius}C");
                    Resolver.Log.Info($"Temperature Old Value {result.Old?.Celsius}C");
                },
                filter: null
            );
            tmp102.Subscribe(consumer);

            tmp102.Updated += (object sender, IChangeResult<Meadow.Units.Temperature> e) =>
            {
                Resolver.Log.Info($"Temperature Updated: {e.New.Celsius:N2}C");
            };

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var temp = await tmp102.Read();
            Resolver.Log.Info($"Current temperature: {temp.Celsius} C");

            tmp102.StartUpdating(TimeSpan.FromSeconds(1));
        }

        //<!=SNOP=>
    }
}