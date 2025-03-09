import axios from "axios";

export const fetchNotes = async (filter) => {
    try{
        var response = await axios.get("https://localhost:44308/notes", {
            params: {
                search: filter?.search,
                sortItem: filter?.sortItem,
                sortOrder: filter?.sortOrder,
            },
        });

        return response.data.notes;
    }
    catch(e){
        console.error(e);
    }
}

export const createQuestion = async (note) => {
    try{
        var response = await axios.post("https://localhost:44308/notes", note);

        return response.status;
    }
    catch(e){
        console.error(e);
    }
}