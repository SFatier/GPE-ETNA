package Dal;

import java.sql.Connection;
import java.sql.Driver;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.ArrayList;

import Interface.IAccountDriveRepository;
import fr.gpe.object.AccountDrive;

public class ReferentielManager {
	
	private static Connection Conn = null;
	private static ReferentielManager instance = null;
	
	private IAccountDriveRepository _AccountDriveRepository;
	//Declarer les interfaces
	
	private ReferentielManager(){
		StartConnection();
		_AccountDriveRepository = new AccountDriveRepository(Conn);
		//ajouter les interfaces ici
	}
	
	//Singleton
	public synchronized static ReferentielManager Instance()
	{
		//Synchronisation Globale
		if(instance == null)
		{		
			instance = new ReferentielManager();
		}
			
		return instance;
	}

	
	public static Connection StartConnection () {
		try {
			Class<?> driverClass = Class.forName("com.mysql.cj.jdbc.Driver");
			DriverManager.registerDriver((Driver)driverClass.newInstance());
			System.out.println( "Vérification du driver ok\n");
		} catch (InstantiationException | IllegalAccessException | ClassNotFoundException | SQLException e) {
			System.out.println("Vérification du driver impossible.");
		}
		
		try {
			// DriverManager: The basic service for managing a set of JDBC drivers.
			Conn = DriverManager.getConnection("jdbc:mysql://127.0.0.1:3308/bbgpe?useUnicode=true&useJDBCCompliantTimezoneShift=true&useLegacyDatetimeCode=false&serverTimezone=UTC","root", "");
			if (Conn != null) {
				System.out.println( "Base de donnée connecté\n");
			} else {
				System.out.println( "Echec connection Base de donnée");
			}
		} catch (SQLException ex) {
		    System.out.println("SQLException: " + ex.getMessage());
		    System.out.println("SQLState: " + ex.getSQLState());
		    System.out.println("VendorError: " + ex.getErrorCode());
		}
		return Conn;
	}
	
	public static void closeConnection(){
		try{
			if(Conn!=null)
				Conn.close();
		}catch(SQLException se){
		}
		System.out.println("Connection closed");
	}
	
	public ArrayList<AccountDrive> GetAccountDriveByID(int id)
	{
		return _AccountDriveRepository.FindAccountDriveByIdCloud(id);
	}
	
}