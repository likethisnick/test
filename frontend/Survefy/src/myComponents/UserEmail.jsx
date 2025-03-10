import React from 'react';

function UserEmail({ user }) {
  if (user === '') {
    return null;
  }
  
  return <div>Hello, {user}</div>;
}

export default UserEmail;
