import axios from "axios";

export const loginUser = async ( {email, pass, rememberme}) => {
    try{
        let loginurl ="";
        if(rememberme == true) {
            loginurl = "/login?useCookies=true"
        }
        else {
            loginurl = "/login?useSessionCookies=true"
        }
        const response = await fetch(loginurl, {
            method: "POST",
            headers: { 
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({
              email: email,
              password: pass,
            }),
          }).then((data) => {
            console.log(data);
            if (data.ok)
                return email;
            else
              return "";
          })
          .catch((error) => {
            console.error(error);
            console.log("Error login");
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