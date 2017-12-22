package Interface;

import java.util.ArrayList;

import fr.gpe.object.AccountDrive;

public interface IAccountDriveRepository  {
	public  ArrayList<AccountDrive> FindAccountDriveByIdCloud(int id);
}
