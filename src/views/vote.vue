<template>
    <div class="vote-page d-flex flex-column">
      <button @click="logout" class="logout-btn btn btn-secondary">登出</button>
      <div class="vote-form-container flex-grow-1 d-flex justify-content-center align-items-center">
        <div class="vote-form row justify-content-center align-items-center">
          <div class="col-4 col-md-4 p-4" @click="selectOption(1)">
            <div :class="['vote-option', { grayscale: selectedOption !== 1 }]">
              <div class="option-number">1</div>
              <img src="https://memeprod.ap-south-1.linodeobjects.com/user-template/4d4165bd43d74d3475988542af33e503.png" alt="Option 1" class="option-image img-fluid">
            </div>
          </div>
          <div class="col-4 col-md-4 p-4" @click="selectOption(2)">
            <div :class="['vote-option', { grayscale: selectedOption !== 2 }]">
              <div class="option-number">2</div>
              <img src="https://memeprod.ap-south-1.linodeobjects.com/user-template/4d4165bd43d74d3475988542af33e503.png" alt="Option 2" class="option-image img-fluid">
            </div>
          </div>
          <div class="col-4 col-md-4 p-4" @click="selectOption(3)">
            <div :class="['vote-option', { grayscale: selectedOption !== 3 }]">
              <div class="option-number">3</div>
              <img src="https://memeprod.ap-south-1.linodeobjects.com/user-template/4d4165bd43d74d3475988542af33e503.png" alt="Option 3" class="option-image img-fluid">
            </div>
          </div>
          <button @click="submitVote" class="submit-btn btn btn-primary mt-3 w-25">投票</button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import Cookies from 'js-cookie';
  import axios from 'axios';
  import { ref, onMounted } from 'vue';
  import { useRouter } from 'vue-router';
  
  export default {
    setup() {
      const selectedOption = ref(null);
      const router = useRouter();
  
      const selectOption = (option) => {
        selectedOption.value = option;
      };
  
      const submitVote = async () => {
        const idNumber = Cookies.get('idNumber');
        const password = Cookies.get('password');
        if (!idNumber || !password) {
          alert('無法獲取用戶信息，請重新登入');
          router.push('/');
          return;
        }
  
        if (selectedOption.value !== null) {
          try {
            const response = await axios.post('http://localhost:5073/Vote', {
              idNumber: idNumber,
              voteChoice: selectedOption.value,
            });
            if (response.data.success) {
              alert('投票已完成，將會在3秒後自動登出');
              setTimeout(() => {
                Cookies.remove('idNumber');
                Cookies.remove('password');
                router.push('/');
              }, 3000); // 3秒後重定向到登入頁面
            } else {
              alert(response.data.error);
            }
          } catch (error) {
            console.error('投票失敗', error);
            alert('投票失敗，請稍後再試');
          }
        } else {
          alert('請選擇一個選項');
        }
      };
  
      const logout = () => {
        alert('已登出');
        Cookies.remove('idNumber');
        Cookies.remove('password');
        router.push('/');
      };
  
      onMounted(() => {
        setTimeout(() => {
          Cookies.remove('idNumber');
          Cookies.remove('password');
          router.push({ path: '/', query: { expired: true } });
        }, 10 * 60 * 1000); // 10 分鐘後重定向到登入頁面
      });
  
      return {
        selectedOption,
        selectOption,
        submitVote,
        logout,
      };
    },
  };
  </script>
  
  <style>
  body {
    margin: 0;
    padding: 0; /* Ensure the content is not hidden under the navbar */
  }
  
  .vote-page {
    position: relative;
    height: 100vh;
    background: #a0a0f0;
  }
  
  .btn {
    background-color: #111;
    border: solid 0.5px #555;
  }
  
  .btn:hover {
    background-color: #134;
  }
  
  .logout-btn {
    position: absolute;
    top: 20px;
    left: 20px;
    z-index: 1;
  }
  
  .vote-form-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-grow: 1;
  }
  
  .vote-form {
    border-radius: 10px;
    background-color: #f0f0f0;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-grow: 1;
    flex-wrap: wrap;
    padding: 100px 0;
    max-width: 800px;
    opacity: 0.9;
  }
  
  .vote-option {
    position: relative;
    border: 2px solid transparent;
    cursor: pointer;
    transition: filter 0.3s;
    aspect-ratio: 10 / 16;
  }
  
  .vote-option img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    display: block;
    margin: 0 auto;
    min-width: 30px;
  }
  
  .vote-option .option-number {
    position: absolute;
    top: 10px;
    left: 10px;
    font-size: 24px;
    font-weight: bold;
    color: white;
    background: rgba(0, 0, 0, 0.5);
    padding: 5px;
    border-radius: 50%;
  }
  
  .vote-option.grayscale {
    filter: grayscale(100%);
  }
  
  .vote-option.grayscale:hover {
    filter: grayscale(0%);
  }
  </style>