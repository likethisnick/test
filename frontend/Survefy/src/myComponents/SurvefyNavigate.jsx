import * as React from 'react';
import { alpha, styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Container from '@mui/material/Container';
import Divider from '@mui/material/Divider';
import MenuItem from '@mui/material/MenuItem';
import Drawer from '@mui/material/Drawer';
import MenuIcon from '@mui/icons-material/Menu';
import CloseRoundedIcon from '@mui/icons-material/CloseRounded';
import ColorModeIconDropdown from '../../shared-theme/ColorModeIconDropdown';
import Sitemark from '../components/SitemarkIcon';
import { getMe } from '@/services/getme';
import AuthorizeView from './AuthorizeView';
import UserEmail from './UserEmail';
import LogoutLink from './LogoutLink';
import { useLocation } from 'react-router-dom';


const UserContext = React.createContext({});

const StyledToolbar = styled(Toolbar)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  flexShrink: 0,
  padding: '8px 12px'
}));


export default function SurvefyNagivate(props) {
  const location = useLocation();
  const [open, setOpen] = React.useState(false);
  const [user, setUser] = React.useState(props.user ? props.user : "");

  const toggleDrawer = (newOpen) => () => {
    setOpen(newOpen);
  };

  React.useEffect(() => {
    setUser(props.user ? props.user : "");
  }, [props.user]);

  React.useEffect(() => {
    getMe().then(response => {
      if(response===undefined){
        setUser("");
      }
      else{
        setUser(response);
      }
    });
  }, [location]);

  const handleLogout = () => {
    setUser("");
  };

  return (
    
      <AppBar
        position="fixed"
        enableColorOnDark
        sx={{ backgroundColor: '#0004FF' }}
      >
        <Container maxWidth="lg" sx={{ height: '64px'  }}>
          <StyledToolbar disableGutters sx={{ height: '64px' }}>
            <Box sx={{ flexGrow: 1, display: 'flex', alignItems: 'center', px: 0 }}>
              <Sitemark />
              <Box sx={{ display: { xs: 'none', md: 'flex' }, px: 5 }}>
                <Button variant="text" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' } }} size="small" >
                <a href="/">Home</a>
                </Button>
                <Button variant="text" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' } }} size="small"
                  onClick={getMe}
                >
                  Test
                </Button>
                <Button variant="text" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' }, minWidth: 0 }} size="small">
                  <a href="/blog">Blog</a>
                </Button>
                <Button variant="text" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' }, minWidth: 0,display: user !== "" ? 'flex' : 'none',
                  }} size="small">
                  <a href="/CreateSurvey">Create survey</a>
                </Button>

              </Box>
            </Box>
            <Box
              sx={{
                display: { xs: 'none', md: 'flex' },
                gap: 1,
                alignItems: 'center',
              }}
            >
              <UserEmail user={user}/>
              <Button color="primary" variant="text" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' },
                display: user !== "" ? 'none' : 'flex', }}>
              <a href="/signin">Sign in</a>
              </Button>
              <Button color="primary" variant="outlined" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' },
                display: user !== "" ? 'none' : 'flex', }}>
                Sign up
              </Button>
              <Button onClick={handleLogout} color="primary" variant="text" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' },
                display: user !== "" ? 'flex' : 'none', }}>
                <LogoutLink>Logout</LogoutLink>
              </Button>
              {/*<ColorModeIconDropdown />*/}
            </Box>
            <Box sx={{ display: { xs: 'flex', md: 'none' }, gap: 1 }}>
              <ColorModeIconDropdown size="medium" />
              <IconButton aria-label="Menu button" onClick={toggleDrawer(true)}>
                <MenuIcon />
              </IconButton>
              <Drawer
                anchor="top"
                open={open}
                onClose={toggleDrawer(false)}
                PaperProps={{
                  sx: {
                    top: 'var(--template-frame-height, 0px)',
                  },
                }}
              >
                <Box sx={{ p: 2 }}>
                  <Box
                    sx={{
                      display: 'flex',
                      justifyContent: 'flex-end',
                    }}
                  >
                    <IconButton onClick={toggleDrawer(false)}>
                      <CloseRoundedIcon />
                    </IconButton>
                  </Box>
                  <MenuItem>
                    <Button variant="text" color="info" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' } }}>
                      <a href="/">Home</a>
                    </Button>
                  </MenuItem>
                  <MenuItem>
                    <Button variant="text" color="info" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' }, minWidth: 0 }}>
                      <a href="/blog">Blog</a>
                    </Button>
                  </MenuItem>
                  <Divider sx={{ my: 3 }} />
                  <MenuItem>
                    <Button color="primary" variant="contained" fullWidth sx={{ color: 'rgb(255, 255, 255)' }}>
                      Sign up
                    </Button>
                  </MenuItem>
                  <MenuItem>
                    <Button color="primary" variant="outlined" fullWidth sx={{ color: 'rgb(255, 255, 255)' }}>
                      Sign in
                    </Button>
                  </MenuItem>
                </Box>
              </Drawer>
            </Box>
          </StyledToolbar>
        </Container>
      </AppBar>
  );
}
