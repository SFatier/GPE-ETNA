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
import com.dropbox.core.v1.DbxEntry;
import com.dropbox.core.v1.DbxEntry.Folder;
import com.dropbox.core.v2.DbxClientV2;
import com.dropbox.core.v2.fileproperties.DbxUserFilePropertiesRequests;
import com.dropbox.core.v2.files.DbxUserFilesRequests;
import com.dropbox.core.v2.files.ListFolderErrorException;
import com.dropbox.core.v2.files.ListFolderResult;
import com.dropbox.core.v2.files.Metadata;
import com.dropbox.core.v2.users.FullAccount;

import fr.etnagpe.constant.Constant;

public class DropBoxConnexion {
	static DbxAppInfo dbxAppInfo;
	static DbxRequestConfig dbxRequestConfig;
	static DbxWebAuthNoRedirect dbxWebAuthNoRedirect ;
	
	/**
	 * URL redirected
	 * @return
	 */
	public static String SendURLAuth() {
		dbxAppInfo = new DbxAppInfo(Constant.DROPBOX_KEY, Constant.DROPBOX_SECRET);
		dbxRequestConfig = new DbxRequestConfig("GPE/1.0", Locale.getDefault().toString());
		dbxWebAuthNoRedirect = new DbxWebAuthNoRedirect(dbxRequestConfig, dbxAppInfo);
		String authorizeUrl = dbxWebAuthNoRedirect.start();
		return authorizeUrl;
	}
	
	/**
	 * Récupère le token
	 * @return
	 */
	public static String GetToken() {
		//System.out.println("2. Récupérer le code et copier dans la console d'eclipse");
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
	 * Récupère les informations clients dans l'application via le token le client
	 * @param ACCESS_TOKEN
	 * @return
	 */
	public  DbxClientV2 ConnectClient (String ACCESS_TOKEN) {
		DbxRequestConfig config = new DbxRequestConfig("GEDETNA", "fr_FR");
        DbxClientV2 client = new DbxClientV2(config, ACCESS_TOKEN);       
        return client;
	}
	
	/**
	 * Récupère les informations du client
	 * @param client
	 * @return
	 * @throws DbxApiException
	 * @throws DbxException
	 */
	public String GetCurrentAccount(DbxClientV2 client) throws DbxApiException, DbxException {
		 FullAccount account = client.users().getCurrentAccount();
	     return account.getName().getDisplayName();
	}
	
	/**
	 * Liste les files disponiblent dans le dropbox du client
	 * @param client
	 * @return
	 * @throws ListFolderErrorException
	 * @throws DbxException
	 */
	public ListFolderResult ListFiles(DbxClientV2 client) throws ListFolderErrorException, DbxException {
		  ListFolderResult result = client.files().listFolder("");
	      return result;
	}
	
	/**
	 * Chercher un fichier correspondant au nom envoyé
	 * @param client
	 * @param Word
	 * @return
	 * @throws ListFolderErrorException
	 * @throws DbxException
	 */
	public String SearchFilesByName(DbxClientV2 client, String Word) throws ListFolderErrorException, DbxException {
		ListFolderResult result = client.files().listFolder("");
		boolean isFind = false;
		int i = 0;
		String strjson = "{\"record\":[";
		 for (Metadata metadata : result.getEntries()) {
			 if ( SearchStringEmp(metadata.getName(), Word) == true) {				 
	             if (isFind && result.getEntries().size() - 1 != i) {
	            	 strjson += ",";          	 
	             }
	             strjson += "{\"pathDisplay\":\""+ metadata.getPathDisplay() + "\",";
	             strjson += "\"nom\":\""+ metadata.getName() + "\",";
	             strjson += "\"type_icone\":\""+ GetTypeMime(metadata.getName())  + "\",";
	             strjson += "\"parentSharedFolderId\":\""+ metadata.getParentSharedFolderId() + "\",";
	             strjson += "\"pathLower\":\""+ metadata.getPathLower() + "\" }"; 
	             isFind = true;
			 }
			 i++;
         }
		 strjson += "]}";
	    return strjson;
	}

	/**
	 * Recherche si un mot existe dans un string 
	 * @param nomCourt
	 * @param Word
	 * @return
	 */
	private boolean SearchStringEmp (String nomCourt, String Word){
		      int intIndex = nomCourt.replace(" ", "").toLowerCase().indexOf(Word.toLowerCase());
						  
		      if(intIndex == - 1 ) {
		         return false;
		      } else {
		         return true;
		      }
		}
	
	/**
	 * Récupère l'extension et renvoie le type d'icone a affiché
	 * @param fileName
	 * @return
	 */
	public String GetTypeMime (String fileName) {
		String img = "";
		String extension = "";
		
		int i = fileName.lastIndexOf('.');
		if (i > 0) {
		    extension = fileName.substring(i+1);
		}
		
		switch (extension)
		{
			case "jpg" : img = "image";break;
			case "png" : img = "image";break;
			case "bmp" : img = "image";break;
			case "gif" : img = "image";break;
			case "jpeg" : img = "image";break;
			case "psd" : img = "image";break;
			case "ai" : img = "image";break;
			case "zip" : img = "archive";break;
			case "rar" : img = "archive";break;
			case "7z" : img = "archive";break;	
			case "mp3" : img = "audio";break;
			case "wav" : img = "audio";break;
			case "aif" : img = "audio";break;
			case "avi" : img = "video";break;
			case "mpg" : img = "video";break;
			case "mpeg" : img = "video";break;
			case "mov" : img = "video";break;
			case "mp4" : img = "video";break;
			case "html" : img = "code";break;
			case "xml" : img = "code";break;
			case "htm" : img = "code";break;
			case "xls" : img = "excel";break;
			case "xlsx" : img = "excel";break;
			case "xlsm" : img = "excel";break;
			case "csv" : img = "excel";break;
			case "pdf" : img = "pdf";break;
			case "ppt" : img = "powerpoint";break;
			case "pps" : img = "powerpoint";break;
			case "pot" : img = "powerpoint";break;
			case "pptx" : img = "powerpoint";break;
			case "ppsx" : img = "powerpoint";break;
			case "potx" : img = "powerpoint";break;
			case "doc" : img = "word";break;
			case "docx" : img = "word";break;
			case "txt" : img = "text";break;
			case "rtf" : img = "text";break;
			case "" : img = "folder"; break;
		}
		return img;		
	}
}
