using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.GameMessages;
using CommonLibrary;

namespace BattleRoayleServer
{
	/// <summary>
	/// делегат для создания события загрузки формы клиента
	/// </summary>
	public delegate void GamerIsLoaded(INetworkClient client);

	/// <summary>
	/// делегат для создания события о том, что игровая логика комнаты завершила свою работу
	/// </summary>
	public delegate void RoomLogicEndWork(IRoomLogic roomLogic);

	/// <summary>
	/// Делегат для создания события о том, что игровая комната завершила свою работу
	/// </summary>
	/// <param name="room"></param>
	public delegate void RoomEndWork(IRoom room);

	public delegate void NetorkClientDisconnect(INetworkClient client);

	public delegate void GetViewMsg(ulong ID, IMessage msg);

	public delegate void HappenedEndGame();

}
