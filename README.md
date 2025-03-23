# PlayFab-
An advanced PlayFab API so you can interact with playfab better with an editor too
working on wiki on how to use

# Credits

## OverlayCS - Main Coder
## Joker Josh - Inspiration & Some Code

## How To Use

Login To Account

```csharp
public void Start()
{
  PlayFabPlusCore.LoginWithCustomID(success=>
  {
    SomeSuccessFunction();
  )}, error=>
  {
    SomeErrorFunction();
  });
}
```

Get Virtual Currency


you have to make an instace of the coin service
```cs
public CoinService coins;
```

then you can call the only function right now

```cs
coins.GetVirtualCurreny((amount)=>
{
    //set some text or value to the amount
}(Error)=>
{
  //Error is always 0 and will return 0
});
```
