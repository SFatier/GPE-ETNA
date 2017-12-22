package Dal;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;

import Interface.IAccountDriveRepository;
import fr.gpe.object.AccountDrive;

public class AccountDriveRepository implements IAccountDriveRepository {

	private Connection Conn = null;
	private Statement statement = null;
	private ResultSet resultSet = null;
	
	public AccountDriveRepository(Connection conn) {
		Conn = conn;
	}

	public ArrayList<AccountDrive> FindAccountDriveByIdCloud(int id) {
		ArrayList<AccountDrive> ListAccountDrive = new ArrayList<AccountDrive>();
		AccountDrive accountDrive = new AccountDrive();
		try {			
			statement = Conn.createStatement();
			resultSet = statement.executeQuery("SELECT ID, EMAIL, MOTDEPASSE, TOKEN FROM AccountDrive WHERE ID =" + id);
			while(resultSet.next()) {
				accountDrive.setId(resultSet.getInt("ID"));
				//accountDrive.setNom(resultSet.getString("NOM"));
				accountDrive.setEmail(resultSet.getString("EMAIL"));
				accountDrive.setMdp(resultSet.getString("MOTDEPASSE"));
				accountDrive.setToken(resultSet.getString("TOKEN"));
				ListAccountDrive.add(accountDrive);
			}
			System.out.println("Account Drive a été récupéré avec success");
		} catch (SQLException ex) {
			System.out.println("L'utilisateur n'a été récupéré. Message : "+ ex.getMessage());
		}finally {
			try {
				Conn.close();
			} catch (SQLException e) {
				System.out.println("Erreur de fermeture de la connexion");
			}
		}
		
		return ListAccountDrive;		
	}
}
