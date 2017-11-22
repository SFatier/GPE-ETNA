package Model;

//Juste un objet
public class File extends BaseObject {

	public String FileName;
	public String ID_File;

	public String getFileName() {
		return FileName;
	}

	public void setFileName(String fileName) {
		FileName = fileName;
	}

	public String getID_File() {
		return ID_File;
	}

	public void setID_File(String iD_File) {
		ID_File = iD_File;
	}
}
