import * as React from 'react';
import { alpha, styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';

const UserContext = React.createContext({});

const StyledToolbar = styled(Toolbar)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  flexShrink: 0,
  padding: '8px 12px'
}));


export default function AuthorizeView({ children }) {
  const [authorized, setAuthorized] = React.useState(false);
  const [loading, setLoading] = React.useState(true);
  const[user,setUser] = React.useState("");

  const [open, setOpen] = React.useState(false);

  React.useEffect(() => {
    let retryCount = 0;
    let maxRetries = 10;
    let delay = 1000;

    function wait(delay) {
      return new Promise((resolve) => setTimeout(resolve,delay))
    }

    async function fetchWithRetry(url, options)
    {
      try{
        let response = await fetch(url, options);
        if(response.status == 200)
        {
          console.log("Authorized");
          let j = await response.json();
          setUser(j.email);
          setAuthorized(true);
          return response;
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
        retryCount++;
        if(retryCount>maxRetries) {
          console.log("unauth");
        }
        else {
          await wait(delay);
          return fetchWithRetry(url, options);
        }
      }
    }

    fetchWithRetry("/pingauth", {
      method: "GET"
    }).catch((error) => {
      console.log("unauth");
    })
    .finally(() => {
      setLoading(false);
    });
  }, []);
  

    if(authorized && !loading) {
      const newChildren = React.cloneElement(children, { user: user });
      return (
        <div>{newChildren}</div>
      )
    }
    else {
      return (
        <div>{children}</div>
      )
    }
}
