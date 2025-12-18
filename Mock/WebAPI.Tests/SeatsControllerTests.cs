using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    private Mock<SeatsService> _seatsService;
    private Mock<SeatsController> _seatsController;

    [TestInitialize]
    public void Init()
    {
        _seatsService = new Mock<SeatsService>();

        _seatsController = new Mock<SeatsController>(_seatsService.Object) { CallBase = true };
        _seatsController.Setup(t => t.UserId).Returns("2");
    }

    [TestMethod]
    public void ReserveSeatAllSeatsTakenTest()
    {
        _seatsService.Setup(s => s.ReserveSeat(It.IsAny<string>(),It.IsAny<int>())).Throws(new SeatOutOfBoundsException());
        int seats = 101;

        var actionresult = _seatsController.Object.ReserveSeat(seats);
        Assert.IsNotNull(actionresult);

        var result = actionresult.Result as NotFoundObjectResult;
        Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
        Assert.AreEqual(result.Value, "Could not find " + seats);
    }

    [TestMethod]
    public void ReserveSeatUserAlreadyAsASeatTest()
    {
        _seatsService.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new UserAlreadySeatedException());
        int seats = 99;

        var actionresult = _seatsController.Object.ReserveSeat(seats);
        Assert.IsNotNull(actionresult);

        var result = actionresult.Result as BadRequestResult;
        Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
    }

    [TestMethod]
    public void ReserveSeatAlreadyTakenTest()
    {
        _seatsService.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatAlreadyTakenException());
        int seats = 99;

        var actionresult = _seatsController.Object.ReserveSeat(seats);
        Assert.IsNotNull(actionresult);

        var result = actionresult.Result as UnauthorizedResult;
        Assert.AreEqual(result.StatusCode, StatusCodes.Status401Unauthorized);
    }

    [TestMethod]
    public void ReserveSeatValideTest()
    {
        int seats = 99;
        Seat valideSeat = new Seat { ExamenUserId = "2", Id = 1, Number = seats };
        _seatsService.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Returns(valideSeat);

        var actionresult = _seatsController.Object.ReserveSeat(seats);
        Assert.IsNotNull(actionresult);

        var result = actionresult.Result as OkObjectResult;
        Assert.AreEqual(valideSeat , result.Value);
    }
}
