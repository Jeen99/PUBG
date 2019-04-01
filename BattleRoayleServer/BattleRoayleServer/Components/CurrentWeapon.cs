using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class CurrentWeapon : Component, ICurrentWeapon
	{
		private ICollector inventory;
		private IWeapon currentWeapon;

		public IWeapon GetCurrentWeapon { get => currentWeapon; }

		public CurrentWeapon(IGameObject parent) : base(parent)
		{
			currentWeapon = null;
		}

		public override IMessage State
		{
			get
			{
				if (currentWeapon != null)
				{
					return new CurrentWeaponState(currentWeapon.TypeWeapon);
				}
				else
				{
					return null;
				}
			}
		}

		public override void Dispose()
		{
			inventory = null;
			currentWeapon = null;
		}

		public override void UpdateComponent(IMessage msg)
		{

			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте CurrentWeapon");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.ChoiceWeapon:
					Handler_ChoiceWeapon(msg as ChoiceWeapon);
					break;
				case TypesProgramMessage.MakeShot:
					Handler_MakeShot(msg as MakeShot);
					break;
				case TypesProgramMessage.AddWeapon:
					Handler_AddWeapon(msg as AddWeapon);
					break;
			}
		}

		private void Handler_AddWeapon(AddWeapon msg)
		{
			//если это первое подобранное оружие делаем его выбранным
			if (currentWeapon == null)
			{
				ChangeWeapon(msg.TypeWeapon);
			}
		}
		private void Handler_MakeShot(MakeShot shot)
		{
			if (currentWeapon != null)
			{
				currentWeapon.SendMessage(shot);
				//обновляем объект раньше очереди, тк выстрел операция с наивысшим приорететом
				currentWeapon.Update();
			}
		}

		private void Handler_ChoiceWeapon(ChoiceWeapon msg)
		{
			if (msg.ChooseWeapon != currentWeapon.TypeWeapon)
			{
				ChangeWeapon(msg.ChooseWeapon);
			}
		}

		private void ChangeWeapon(TypesWeapon type)
		{
			currentWeapon = inventory.GetWeapon(type);
			currentWeapon.Holder = Parent;
			//отправляем сообщение об этом
			Parent?.Model?.AddEvent(new ChangedCurrentWeapon(type));
		}

		public override void Setup()
		{
			this.inventory = Parent.Components?.GetComponent<Collector>();
			if (inventory == null)
			{
				Log.AddNewRecord("Ошибка создания компонента CurrentWeapon", "Не получена сслыка на компонент Collector");
				throw new Exception("Ошибка создания компонента CurrentWeapon");
			}
		}
	}
}
