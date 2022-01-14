using System;
using Automation.BDaq;

public class CTT_4_TESTER
{
	uint relayCharger = 0;
	uint relayHV = 0;
	uint relayBattery = 0;
	byte maskCharger = 0x01;
	byte maskHV = 0x02;
	byte maskBattery = 0x04;
	byte maskWpcCharger = 0x08;

	InstantDoCtrl doCtrl;
	public P4Relay(InstantDoCtrl doCtrl,int deviceNumber)
	{
		this.doCtrl = doCtrl;
		this.doCtrl.SelectedDevice = new DeviceInformation(deviceNumber);
	}

	public ErrorCode init()
	{
		return this.doCtrl.Write(0, (byte)0x00);

	}

	public ErrorCode toggleCharger()
	{
		ErrorCode err;
		byte portData = 1;
		err = this.doCtrl.Read(0,out portData);
		if (relayCharger == 0)
		{
			relayCharger = 1;
			byte mask = 0x01;
			portData = (byte)(portData | mask);
			err = this.doCtrl.Write(0, portData );
		}
		else
		{
			relayCharger = 0;
			byte mask = 0x01;
			portData = (byte)(portData & ~mask);
			err = this.doCtrl.Write(0, portData);
		}
		return err;
	}

	public ErrorCode toggleHV()
	{
		ErrorCode err;
		byte portData = 1;
		byte mask = 0x02;
		err = this.doCtrl.Read(0, out portData);
		if (relayHV == 0)
		{
			relayHV = 1;
			portData = (byte)(portData | mask);
			err = this.doCtrl.Write(0, portData);
		}
		else
		{
			relayHV = 0;
			portData = (byte)(portData & ~mask);
			err = this.doCtrl.Write(0, portData);
		}
		return err;
	}

	public ErrorCode toggleBattery()
	{
		ErrorCode err;
		byte portData = 1;
		byte mask = 0x04;
		err = this.doCtrl.Read(0, out portData);
		if (relayBattery == 0)
		{
			relayBattery = 1;
			portData = (byte)(portData | mask);
			err = this.doCtrl.Write(0, portData);
		}
		else
		{
			relayBattery = 0;
			portData = (byte)(portData & ~mask);
			err = this.doCtrl.Write(0, portData);
		}
		return err;
	}

	public ErrorCode setStandby()
	{
		ErrorCode err;
		byte portData = 0x00;
		err = this.doCtrl.Write(0, portData);

		return err;
	}

	public ErrorCode setCharging()
	{
		ErrorCode err;
		byte portData =(byte)( maskBattery | maskCharger);
		err = this.doCtrl.Write(0, portData);

		return err;
	}

	public ErrorCode setWpcCharging()
	{
		ErrorCode err;
		byte portData = (byte)(maskBattery | maskWpcCharger);
		err = this.doCtrl.Write(0, portData);

		return err;
	}

	public ErrorCode setHV()
	{
		ErrorCode err;
		byte portData = maskHV;
		err = this.doCtrl.Write(0, portData);

		return err;
	}

}
