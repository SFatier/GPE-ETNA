package fr.etnagpe.dropbox;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Locale;

import com.dropbox.core.DbxApiException;
import com.dropbox.core.DbxAppInfo;
import com.dropbox.core.DbxAuthFinish;
import com.dropbox.core.DbxException;
import com.dropbox.core.DbxRequestConfig;
import com.dropbox.core.DbxWebAuthNoRedirect;
import com.dropbox.core.v2.DbxClientV2;
import com.dropbox.core.v2.files.ListFolderErrorException;
import com.dropbox.core.v2.files.ListFolderResult;
import com.dropbox.core.v2.files.Metadata;
import com.dropbox.core.v2.users.FullAccount;

import fr.etnagpe.constant.Constant;

public class DropBoxConnexion {

	/**
	 * Récupère le token
	 * */	
	public String GetToken() {
		DbxAppInfo dbxAppInfo = new DbxAppInfo(Constant.DROPBOX_KEY, Constant.DROPBOX_SECRET);
		DbxRequestConfig dbxRequestConfig = new DbxRequestConfig("GPE/1.0", Locale.getDefault().toString());
		DbxWebAuthNoRedirect dbxWebAuthNoRedirect = new DbxWebAuthNoRedirect(dbxRequestConfig, dbxAppInfo);
		String authorizeUrl = dbxWebAuthNoRedirect.start();
		System.out.println("1. Autorisation de l'utilisateur, allez sur le lien et connectez-vous : " + authorizeUrl);
		System.out.println("2. Récupérer le code et copier dans la console d'eclipse");
		String dropboxAuthCode = null;
		DbxAuthFinish authFinish = null;
		
		try {
			dropboxAuthCode = new BufferedReader(new InputStreamReader(System.in)).readLine().trim();
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
		
		try {
			authFinish = dbxWebAuthNoRedirect.finish(dropboxAuthCode);
		} catch (DbxException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		String authAccessToken = authFinish.getAccessToken();
		System.out.println(authAccessToken);
		return authAccessToken;
	}
	
	/**
	 * Récupère les informations clients dans l'application via le token le clien 
	 * **/
	public  DbxClientV2 ConnectClient (String ACCESS_TOKEN) {
		DbxRequestConfig config = new DbxRequestConfig("GEDETNA", "fr_FR");
        DbxClientV2 client = new DbxClientV2(config, ACCESS_TOKEN);       
        return client;
	}
	
	/**
	 * Récupère les informations du client
	 * **/
	public String GetCurrentAccount(DbxClientV2 client) throws DbxApiException, DbxException {
		 FullAccount account = client.users().getCurrentAccount();
	     return account.getName().getDisplayName();
	}
	
	/**
	 * Liste les files disponiblent dans le dropbox du client
	 * **/
	public ListFolderResult ListFiles(DbxClientV2 client) throws ListFolderErrorException, DbxException {
		  ListFolderResult result = client.files().listFolder("");
	       /*s while (true) {
	            for (Metadata metadata : result.getEntries()) {
	                System.out.println(metadata.getPathLower());
	            }

	            if (!result.getHasMore()) {
	                break;
	            }

	            result = client.files().listFolderContinue(result.getCursor());
	        }*/
	        return result;
	}
}
