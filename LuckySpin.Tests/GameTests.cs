namespace LuckySpin.Tests;
using LuckySpin.Models;
using Xunit;

public class GameTests
{
    private readonly Spin _spin;
    private readonly Game _game;
    private readonly Player _player;
      public GameTests()
    {
        _player = new Player()
        {
            FirstName = "Test",
            Luck = 5,
            Balance = 10.0m
        };
        _game = new Game{
            Player = _player
        };
        _spin = new Spin();
    }

    [Fact]
    public void TurnCostsFiftyCents()
    {
        Spin spin = new Spin();
        while (spin.isWinning(_player))
        {
            //create new spin until not winning
            spin = new Spin();
        } 
        decimal balanceBeforeTurn = _player.Balance;
        _game.PlayTurn(spin);  
        Assert.Equal(balanceBeforeTurn - 0.5m, _player.Balance);
    }
    [Fact]
    public void WinningPaysOutOneDollar(){
        Spin spin = new Spin();
        while (!spin.isWinning(_player))
        {
            //create new spin until you get a winning one
            spin = new Spin();
        } 
        decimal balanceBeforeTurn = _player.Balance;
        _game.PlayTurn(spin);  
        Assert.Equal(balanceBeforeTurn + 0.5m, _player.Balance);
    }
    [Fact]
    public void SpinIsAddedToGameSpinsList()
    {
        int initialCount = _game.Spins.Count();
        _game.PlayTurn(_spin);
        Assert.Equal(initialCount + 1, _game.Spins.Count());
    }
    [Fact]
    public void GameStatusSetToSpinningWhenNotWinning()
    {
        Spin spin = new Spin();
        while (spin.isWinning(_player))
        {
            //create new spin until not winning
            spin = new Spin();
        }   
        _game.PlayTurn(spin);  
        Assert.Equal(GameStatus.Spinning, _game.Status);
    }
    [Fact]
    public void GameStatusSetToGameOverWhenInsufficientBalance()
    {
        _player.Balance = 0.25m;
        _game.PlayTurn(_spin);
        Assert.Equal(GameStatus.GameOver, _game.Status);
    }
}