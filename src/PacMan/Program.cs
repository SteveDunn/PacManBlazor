using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PacMan.GameComponents;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.GameActs;
using PacMan.GameComponents.Ghosts;
using PacMan.GameComponents.Requests;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace PacMan
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton<IGame, Game>();
            builder.Services.AddSingleton<IGameStorage, GameStorage>();
            builder.Services.AddSingleton<IHumanInterfaceParser, HumanInterfaceParser>();
            builder.Services.AddSingleton<ISoundLoader, SoundLoader>();
            builder.Services.AddSingleton<IGameSoundPlayer, GameSoundPlayer>();
            
            builder.Services.AddSingleton<IAct, AttractAct>();
            builder.Services.AddSingleton<IAct, GameAct>();

            builder.Services.AddSingleton<IAct, BigPacChaseAct>();
            builder.Services.AddSingleton<IAct, GameOverAct>();
            builder.Services.AddSingleton<IAct, GhostTearAct>();
            builder.Services.AddSingleton<IAct, LevelFinishedAct>();
            builder.Services.AddSingleton<IAct, NullAct>();
            builder.Services.AddSingleton<IAct, PacManDyingAct>();
            builder.Services.AddSingleton<IAct, PlayerIntroAct>();
            builder.Services.AddSingleton<IAct, DemoPlayerIntroAct>();
            builder.Services.AddSingleton<IAct, StartButtonAct>();
            builder.Services.AddSingleton<IAct, TornGhostChaseAct>();
            
            builder.Services.AddSingleton<IAct, PlayerGameOverAct>();

            builder.Services.AddSingleton<IActs, Acts>();

            builder.Services.AddSingleton<IGameStats, GameStats>();

            builder.Services.AddSingleton<IHaveTheMazeCanvases, MazeCanvases>();
            
            builder.Services.AddSingleton<IGhost, Blinky>();
            builder.Services.AddSingleton<IGhost, Pinky>();
            builder.Services.AddSingleton<IGhost, Inky>();
            builder.Services.AddSingleton<IGhost, Clyde>();

            builder.Services.AddSingleton<IPacMan, PacMan.GameComponents.PacMan>();
            
            builder.Services.AddSingleton<ICoinBox, CoinBox>();
            
            builder.Services.AddSingleton<IFruit, Fruit>();
            
            builder.Services.AddSingleton<IGhostCollection, GhostCollection>();
            
            builder.Services.AddSingleton<IStatusPanel, StatusPanel>();
            builder.Services.AddSingleton<IScorePanel, ScorePanel>();
            
            builder.Services.AddSingleton<IMaze, Maze>();

            builder.Services.AddLogging();

            builder.Services.Add(new ServiceDescriptor(
                typeof(IExceptionNotificationService),
                typeof(ExceptionNotificationService),
                ServiceLifetime.Singleton));

            
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            
            Assembly componentsAssembly = typeof(ClassThatLivesInGameComponentsActsAsAMarkerForThisAssemblyForReflection).Assembly;

            builder.Services.AddMediatR(c => c.AsSingleton(),
                thisAssembly,
                componentsAssembly);

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            var host = builder.Build();
            
            await host.RunAsync();
        }
    }

    public class MyMediator : IMediator
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
        {
            throw new NotImplementedException();
        }
    }


    public interface IExceptionNotificationService
    {
        /// <summary>
        /// Raised is an exception occurs. The exception message will be send to the listeners
        /// </summary>
        event EventHandler<string> OnException;
    }

    public class ExceptionNotificationService : TextWriter, IExceptionNotificationService
    {
        private readonly TextWriter _decorated;

        public override Encoding Encoding => Encoding.UTF8;

        /// <summary>
        /// Raised is an exception occurs. The exception message will be send to the listeners
        /// </summary>
        public event EventHandler<string> OnException;

        public ExceptionNotificationService()
        {
            _decorated = Console.Error;
            Console.SetError(this);
        }
        //THis is the method called by Blazor
        public override void WriteLine(string value)
        {
            //notify the listenners
            OnException?.Invoke(this,value);

            _decorated.WriteLine(value);
        }
    }
}
