package fr.gpe.object;

import java.util.Date;

public class AccountDrive {
	private int id;
	private String nom;
	private String ClientID;
	private String ClientSecret;
	private String Email;
	private String mdp;
	private Date DateDeCreation;
	private String Clé;
	private String token;
	
	public int getId() {
		return id;
	}
	public void setId(int id) {
		this.id = id;
	}
	public String getNom() {
		return nom;
	}
	public void setNom(String nom) {
		this.nom = nom;
	}
	public String getClientID() {
		return ClientID;
	}
	public void setClientID(String clientID) {
		ClientID = clientID;
	}
	public String getClientSecret() {
		return ClientSecret;
	}
	public void setClientSecret(String clientSecret) {
		ClientSecret = clientSecret;
	}
	public String getEmail() {
		return Email;
	}
	public void setEmail(String email) {
		Email = email;
	}
	public String getMdp() {
		return mdp;
	}
	public void setMdp(String mdp) {
		this.mdp = mdp;
	}
	public Date getDateDeCreation() {
		return DateDeCreation;
	}
	public void setDateDeCreation(Date dateDeCreation) {
		DateDeCreation = dateDeCreation;
	}
	public String getClé() {
		return Clé;
	}
	public void setClé(String clé) {
		Clé = clé;
	}
	public String getToken() {
		return token;
	}
	public void setToken(String token) {
		this.token = token;
	}
}
