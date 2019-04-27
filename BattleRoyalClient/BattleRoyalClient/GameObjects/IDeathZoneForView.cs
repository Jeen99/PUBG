using System;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	interface IDeathZoneForView:IModelObject
	{
		TimeSpan TimeToChange { get; }
	}
}