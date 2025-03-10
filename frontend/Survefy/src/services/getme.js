import axios from "axios";

export const getMe = async () => {
    try{
        let response = await fetchWithRetry("/pingauth", {
          method: "GET"
        });
        return response;
    }
    catch (err) {
        if (err.response) {
          console.error('Ошибка логина:', err.response.data);
        } else {
          console.error('Ошибка при подключении к серверу');
        }
      }
}

async function fetchWithRetry(url, options)
    {
      try{
        let response = await fetch(url, options);
        if(response.status == 200)
        {
          console.log("Authorized");
          let j = await response.json();
          return j.email;
        }
        else if (response.status == 401)
        {
          console.log("Unauthorized");
          return response;
        }
        else {
          console.log("unauth");
        }
      }
      catch(error) {
        console.log("unauth");
      }
    }