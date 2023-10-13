using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieManagement.DataAccess.Context;
using MovieManagement.DataAccess.Implementation;
using MovieManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagementTests.Repositories
{
    public class ActorRepositoryTests
    {
        /// <summary>
        /// Seeds database for in-memory testing
        /// <param name="firstName">actor first name</param>
        /// <param name="lastName">actor last name</param>
        public MovieManagementDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<MovieManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new MovieManagementDbContext(options);
            dbContext.Database.EnsureCreated();

            if (dbContext.Actors.Count() <= 0)
            {
                dbContext.Actors.AddRange(
                        new Actor { Id = 1, FirstName = "Chuck", LastName = "Norris", SensitiveInformation = "Did not actually walk on the moon without a spacesuit" },
                        new Actor { Id = 2, FirstName = "Ryan", LastName = "Gosling", SensitiveInformation = "Actually has a girlfriend" },
                        new Actor { Id = 3, FirstName = "Ana", LastName = "DeArmas", SensitiveInformation = "Is not answering my calls" });

                dbContext.Movies.AddRange(
                    new Movie { Id = 1, Name = "Black Panther", Description = "Wakanda forever", ActorId = 1 },
                    new Movie { Id = 2, Name = "Blade Runner 2077", Description = "tfw no gf", ActorId = 2 },
                    new Movie { Id = 3, Name = "The Matrix", Description = "which pill?", ActorId = 1 },
                    new Movie { Id = 4, Name = "Barbie", Description = "woke mind virus", ActorId = 3 }
                );

                dbContext.Biographies.Add(
                    new Biography { Id = 1, Description = "Early internet meme.", ActorId = 1 }
                );
            }

            dbContext.SaveChanges();
            return dbContext;
        }

        [Fact]
        public void ActorsRepository_GetActors_ReturnsListOfActors()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;

            //Act
            var actors = unitOfWork.Actor.GetAll();

            //Assert
            actors.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ActorsRepository_GetActors_ReturnsListOfActors_WithoutMovies()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;

            //Act
            var actor = unitOfWork.Actor.GetAll().FirstOrDefault();

            //Assert
            actor.Should().NotBeNull();
            actor.Movies.Should().BeNull();

        }

        [Fact]
        public void ActorsRepository_GetActorsByFullName_ReturnsActorWithGivenName()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;

            //Act
            var actors = unitOfWork.Actor.GetActorsByFullName("Chuck", "Norris");

            //Assert
            actors.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ActorsRepository_GetActorsByFullName_WithInvliadName_ReturnsNoActors()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;

            //Act
            var actors = unitOfWork.Actor.GetActorsByFullName("Chuck", "Yaeger");

            //Assert
            actors.Should().BeNullOrEmpty();
        }

        [Fact]
        public void ActorsRepository_GetActorsWithBiography_ReturnsActorsWithBiography()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;

            //Act
            var actor = unitOfWork.Actor.GetActorsWithBiography().FirstOrDefault();

            //Assert
            actor.Should().NotBeNull();
            actor.Biography.Should().NotBeNull();
        }

        [Fact]
        public void ActorsRepository_GetActorsWithId_ReturnsActorsWithId()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;

            //Act
            var actor = unitOfWork.Actor.GetById(1);

            //Assert
            actor.Should().NotBeNull();
        }

        [Fact]
        public void ActorsRepository_GetActorsWithInvalidId_ReturnsNoActors()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;

            //Act
            var actor = unitOfWork.Actor.GetById(-1);

            //Assert
            actor.Should().BeNull();
        }

        [Fact]
        public void ActorsRepository_RemoveActor_RemovesActorFromDatabase()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;
            var actorToRemove = new Actor { Id = 1, FirstName = "Chuck", LastName = "Norris", SensitiveInformation = "Did not actually walk on the moon without a spacesuit" };

            //Act
            unitOfWork.Actor.Remove(actorToRemove);
            unitOfWork.Save();
            var actor = actorRepository.GetById(1);

            //Assert
            actor.Should().BeNull();
        }

        [Fact]
        public void ActorsRepository_RemoveActorRange_RemovesMultipleActorsFromDatabase()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;
            var actorsToRemove = new List<Actor>
                    {
                        new Actor { Id = 1, FirstName = "Chuck", LastName = "Norris", SensitiveInformation = "Did not actually walk on the moon without a spacesuit" },
                        new Actor { Id = 2, FirstName = "Ryan", LastName = "Gosling", SensitiveInformation = "Actually has a girlfriend" }
                    };

            //Act
            unitOfWork.Actor.RemoveRange(actorsToRemove);
            unitOfWork.Save();
            var actor1 = actorRepository.GetById(1);
            var actor2 = actorRepository.GetById(2);

            //Assert
            actor1.Should().BeNull();
            actor2.Should().BeNull();
        }

        [Fact]
        public void ActorsRepository_AddActor_AddsActorToDatabase()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;
            var actorToAdd = new Actor { Id = 4, FirstName = "Harrison", LastName = "Ford", SensitiveInformation = "Dies to Kylo Ren" };

            //Act
            unitOfWork.Actor.Add(actorToAdd);
            unitOfWork.Save();
            var actor1 = actorRepository.GetById(4);

            //Assert
            actor1.Should().NotBeNull();
        }

        [Fact]
        public void ActorsRepository_AddActorRange_AddsMultipleActorsToDatabase()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;
            var actorsToAdd = new List<Actor>
                    {
                        new Actor { Id = 4, FirstName = "Harrison", LastName = "Ford", SensitiveInformation = "Dies to Kylo Ren" },
                        new Actor { Id = 5, FirstName = "Margo", LastName = "Robbie", SensitiveInformation = "Was in the really bad version of the Expendables" }
                    };

            //Act
            unitOfWork.Actor.AddRange(actorsToAdd);
            unitOfWork.Save();
            var actor1 = actorRepository.GetById(4);
            var actor2 = actorRepository.GetById(5);

            //Assert
            actor1.Should().NotBeNull();
            actor2.Should().NotBeNull();
        }

        [Fact]
        public void ActorsRepository_UpdateActor_UpdatesActorInDatabase()
        {
            //Arranges
            var dbContext = GetDatabaseContext();
            var actorRepository = new ActorRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            unitOfWork.Actor = actorRepository;
            var actorToUpdate = unitOfWork.Actor.GetActorsByFullName("Chuck", "Norris").First();
            actorToUpdate.FirstName = "Charles";

            //Act
            unitOfWork.Actor.Update(actorToUpdate);
            unitOfWork.Save();
            var updatedActor = unitOfWork.Actor.GetActorsByFullName("Charles", "Norris").First();

            //Assert
            updatedActor.Should().NotBeNull();
        }
    }
}
